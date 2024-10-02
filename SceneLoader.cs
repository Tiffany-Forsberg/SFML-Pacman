using System.Text;
using SFML.System;

namespace Pacman
{
    public class SceneLoader
    {
        private readonly Dictionary<char, Func<Entity>> loaders;
        private string currentScene = "", nextScene = "";

        public SceneLoader()
        {
            loaders = new Dictionary<char, Func<Entity>>
            {
                { '#', () => new Wall() },
                { 'g', () => new Ghost() },
                { 'p', () => new Pacman() },
                { '.', () => new Coin() },
                { 'c', () => new Candy() },
            };
        }

        private bool Create(char symbol, out Entity created)
        {
            if (loaders.TryGetValue(symbol, out Func<Entity> loader))
            {
                created = loader();
                return true;
            }

            created = null;
            return false;
        }

        public void Load(string scene) => nextScene = scene;
        public void Reload() => nextScene = currentScene;
        
        public void HandleSceneLoad(Scene scene)
        {
            if (nextScene == "") return;
            scene.Clear();

            string file = $"{AssetManager.AssetPath}/{nextScene}.txt";
            Console.WriteLine($"Loading scene '{file}'");
            
            // Load scene file
            int row = 0;
            foreach (var line in File.ReadLines(file, Encoding.UTF8))
            {    
                string parsed = line.Trim();
                
                if (parsed.Length == 0) continue;

                char[] characters = parsed.ToCharArray();

                for (int col = 0; col < characters.Length; col++)
                {
                    if (Create(characters[col], out Entity created))
                    {
                        created.Position = new Vector2f(col * 18, row * 18);
                        scene.Spawn(created);
                    }
                }

                row++;
            }

            if (!scene.FindByType<GUI>(out _))
            {
                scene.Spawn(new GUI());
            }
            
            currentScene = nextScene;
            nextScene = "";
        }
    }
}
