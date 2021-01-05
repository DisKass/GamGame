using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem<T1, T2> where T1: MonoBehaviour
    where T2: class, IPersistentData<T1>, new()
    
{
    public static void Save(T1 persistentObject, string pathName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + pathName + ".xzod";
        //Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        T2 persistentData = new T2();
        persistentData.Initialize(persistentObject);
        formatter.Serialize(stream, persistentData);
        stream.Close();

    }

    public static T2 Load(string pathName)
    {
        string path = Application.persistentDataPath + "/" + pathName + ".xzod";
        if (CheckSaveFile(pathName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            T2 persistendData = formatter.Deserialize(stream) as T2;
            stream.Close();
            return persistendData;
        } else
        {
            Debug.LogError("Save File Not Found in Path: " + path);
            return null;
        }
    }
    public static void Delete(string pathName)
    {
        string path = Application.persistentDataPath + "/" + pathName + ".xzod";
        File.Delete(path);
    }
    public static bool CheckSaveFile(string pathName)
    {
        string path = Application.persistentDataPath + "/" + pathName + ".xzod";
        return File.Exists(path);
    }
}
