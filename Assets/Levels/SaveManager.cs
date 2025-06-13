using UnityEngine;
using System.IO;

namespace Levels
{
    public class SaveManager
    {
        private static string savePath = Path.Combine(
            Application.persistentDataPath, "playerProgress.json");
    
        public static void SaveProgress(string levelName)
        {
            PlayerProgress progress = new PlayerProgress();
            progress.currentLevelName = levelName;
        
            string json = JsonUtility.ToJson(progress);
            File.WriteAllText(savePath, json);
        }
    
        public static PlayerProgress LoadProgress()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                return JsonUtility.FromJson<PlayerProgress>(json);
            }
            return new PlayerProgress();
        }
    
        public static void DeleteSave()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }
    }
}

namespace Levels
{
    [System.Serializable]
    public class PlayerProgress
    {
        public string currentLevelName = "tutorial";
    }
}
