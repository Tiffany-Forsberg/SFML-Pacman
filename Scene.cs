using SFML.Graphics;
using SFML.Window;

namespace Pacman
{
    public sealed class Scene
    {
        public readonly SceneLoader Loader = new SceneLoader();
        public readonly AssetManager Assets = new AssetManager();
        public readonly EventManager Events = new EventManager();
        
        private readonly List<Entity> entities = new List<Entity>();
        private bool isGameOver = false;

        public Scene()
        {
            Events.GameOver += OnGameOver;
        }

        private void OnGameOver(Scene scene, bool state)
        {
            isGameOver = state;
        }
        
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
                if (!entity.DontDestroyOnLoad)
                {
                    entities.RemoveAt(i);
                    entity.Destroy(this);
                }
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
            Events.HandleEvents(this);
            
            if (isGameOver)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                {
                    Events.PublishGameOver(false);
                    Loader.Reload();
                }
            }
            else
            {
                for (int i = entities.Count - 1; i >= 0; i--)
                {
                    Entity entity = entities[i];
                    entity.Update(this, deltaTime);
                }
                
                for (int i = 0; i < entities.Count;)
                {
                    Entity entity = entities[i];
                    if (entity.Dead) entities.RemoveAt(i);
                    else i++;
                }
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
