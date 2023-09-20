using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public PlayerData SavedPlayerData;

    public SaveData(PlayerData playerData) {
        SavedPlayerData = playerData;
    }
}

public class SaveManager : ManagerBase
{
    public static SaveManager instance;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InitManager() {
        LoadFile();
    }


    public void SaveFile() {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();

        SaveData newSaveData = new SaveData(PlayerManager.instance.GetPlayerData());
        bf.Serialize(file, newSaveData);
        file.Close();
    }

    public void LoadFile() {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        SaveData data = (SaveData)bf.Deserialize(file);
        file.Close();

        PlayerManager.instance.SetPlayerData(data.SavedPlayerData);


        //Debug.Log(data.SavedPlayerData.HighScore);
        //Debug.Log(data.SavedPlayerData.Score);
        //Debug.Log(data.SavedPlayerData.Duration);
    }
}
