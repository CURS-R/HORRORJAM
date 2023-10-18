using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CURSR.Utils
{
    [System.Serializable]
    public class IOFile
    {
        private readonly string _fileName;
        public IOFile(string fileName)
        {
            _fileName = fileName;
        }
        
        public void Save<T>(T dataToSave)
        {
            try
            {
                string filePath = Path.Combine(Application.persistentDataPath, _fileName);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    binaryFormatter.Serialize(fileStream, dataToSave);
                }
            }
            catch (SerializationException e)
            {
                Debug.LogError($"<color=red>Failed to serialize. Reason:</color> {e.Message}");
                throw;
            }
        }

        public T Load<T>() where T : class
        {
            string filePath = Path.Combine(Application.persistentDataPath, _fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        return binaryFormatter.Deserialize(fileStream) as T;
                    }
                }
                catch (SerializationException e)
                {
                    Debug.LogError($"<color=red>Failed to deserialize. Reason:</color> {e.Message}");
                    throw;
                }
            }
            else
            {
                Debug.LogError($"<color=red>File not found at</color> {filePath}");
                return null;
            }
        }
    }
}