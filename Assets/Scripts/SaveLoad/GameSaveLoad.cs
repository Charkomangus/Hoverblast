using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public static class GameSaveLoad
    {

        public static List<GameData> SavedGames = new List<GameData>();

        //it's static so we can call it from anywhere
        public static void Save()
        {
            SavedGames.Add(GameData.CurrentGameData);
            var bf = new BinaryFormatter();
            var file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
            bf.Serialize(file, SavedGames);
            file.Close();
        }

        public static void Load()
        {
            if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
            {
                var bf = new BinaryFormatter();
                var file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
                SavedGames = (List<GameData>)bf.Deserialize(file);
                file.Close();
            }
        }
    }
}
