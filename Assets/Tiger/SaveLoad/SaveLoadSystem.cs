using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CURSR
{
    public static class SaveLoadSystem
    {
        public static void SavePlayerData(PlayerSaveAttributes saveAttributes)
        {
            BinaryFormatter binaryFormater = new BinaryFormatter();
            string filePath = Application.persistentDataPath + "/playerData.save";
            FileStream fileStream = new FileStream(filePath, FileMode.Create);

            PlayerData playerData = new PlayerData(saveAttributes);

            binaryFormater.Serialize(fileStream, playerData);
            fileStream.Close();
        }

        public static PlayerData LoadPlayerData()
        {
            string filePath = Application.persistentDataPath + "/playerData.save";
            if(File.Exists(filePath))
            {
                BinaryFormatter binaryFormater = new BinaryFormatter();
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                PlayerData playerData = binaryFormater.Deserialize(fileStream) as PlayerData;
                fileStream.Close();
                return playerData;
            }
            else
            {
                Debug.LogError($"<color=red>Save file not found in</color> {filePath}");
                return null;
            }
        }
    }
}
