using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
   public static void SavePlayer(PlayerStats player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        //path to the data directory of the operating system:
        string path = Path.Combine(Application.persistentDataPath + "/ana.sexytime");
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerData LoadPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath + "/ana.sexytime");
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            try
            {
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                return data;
                
            }
            catch
            {
                Debug.LogError("Some error happened on deserialisation.");
                stream.Close();
                return null;
            }
            finally
            {
                stream.Close();
            }
            
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
