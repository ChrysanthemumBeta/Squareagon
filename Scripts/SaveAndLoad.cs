using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveAndLoad
{
    public static void SaveCharacter(string name, float[] colour, InventorySaveItem[] Inv)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Profiles/" + name + ".character";
        FileStream stream = new FileStream(path, FileMode.Create);

        CharacterData data = new CharacterData(name, colour, Inv);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    
    public static CharacterData LoadCharacter(string name)
    {
        string path = Application.persistentDataPath + "/Profiles/" + name + ".character";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            CharacterData data = formatter.Deserialize(stream) as CharacterData;

            return data;
        }
        else
        {
            Debug.LogWarning("Save not found in path" + path);
            return null;
        }
    }

    public static string[] GetAllProfiles()
    {
        string path = Application.persistentDataPath + "/Profiles";

        string[] files = Directory.GetFiles(path);
        string[] profiles = new string[files.Length];
        int i = 0;
        foreach (string item in files)
        {
            profiles[i] = Path.GetFileNameWithoutExtension(path + "/" + item);
            i++;
        }
        return profiles;

    }

    public static void DeleteProfile(string name)
    {
        string path = Application.persistentDataPath + "/Profiles/";
        File.Delete(Path.Combine(path, name + ".character"));
    }
}
