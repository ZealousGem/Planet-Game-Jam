using System.IO;
using UnityEngine;

public class SettingsDataManager : Singleton<SettingsDataManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     private SettingsData data = new SettingsData();
    private const string fileName = "SettingsData.json";
    private string persistentPath => Path.Combine(Application.persistentDataPath, fileName);

    public override void Awake()
    {
        base.Awake();
        DataInFile();
    }

    public void setData(SettingsData data)
     {
        this.data = data;

        try
        {
             string json = JsonUtility.ToJson(this.data, true);
             File.WriteAllText(persistentPath, json);
        }

        catch
        {
            Debug.Log("does not exist in webversion");
        } 
            
     }

    public bool DataInFile()
    {
        if (File.Exists(persistentPath))
        {
          try {
            string json = File.ReadAllText(persistentPath);
            if (string.IsNullOrEmpty(json)) return false;
            
            data = JsonUtility.FromJson<SettingsData>(json);
          //  Debug.Log("File found " + Application.persistentDataPath);
            return true;
        } catch {
             Debug.Log("File Corrupted");
            return false;
            // Handle corrupted JSON
        }
        }
         Debug.Log("File not found");
        return false;
         // No file found in either location
    }

    public SettingsData getFileData() => data;
}
