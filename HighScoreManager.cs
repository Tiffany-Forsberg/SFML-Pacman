using System.Text;

namespace Pacman
{
    public sealed class HighScoreManager
    {
        private string systemPath;
        private string path;

        public HighScoreManager()
        {
            systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = Path.Combine(systemPath, "HighScore.txt");
        }        
        
        public int HandleHighScore(int score)
        {
            int currentHighScore = 0;
            
            if (File.Exists(path))
            {
                if (File.ReadLines(path, Encoding.UTF8).Any())
                {
                    currentHighScore = int.Parse(File.ReadLines(path, Encoding.UTF8).ToArray()[0]);
                }
            }
            
            if (score >= currentHighScore)
            {
                WriteScoreToFile(score);
            }
            
            return int.Parse(File.ReadLines(path, Encoding.UTF8).ToArray()[0]);
        }

        public void WriteScoreToFile(int score)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(score);
            }
        }
    }
}
