using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SaveManager : MonoBehaviour
{
    public const string SAVE_NAME = "Save.json";
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindAnyObjectByType<SaveManager>(FindObjectsInactive.Include);
            return _instance;
        }
    }

    private static SaveManager _instance;

    public static UnityAction OnSave;
    public static UnityAction OnLoaded;


    private List<Saveable> saveables = new List<Saveable>();

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("Save"))
            Load();
        else
            OnLoaded?.Invoke();
    }

    public static T GetSaveData<T>() where T : Saveable
    {
        var data = Instance.saveables.FirstOrDefault(s => s.GetType() == typeof(T));
        if (data == null)
        {
            data = (T)Activator.CreateInstance(typeof(T));
            data.WriteDefaults();
            Instance.saveables.Add(data);
        }
        return data as T;
    }

    public static void Save()
    {
        OnSave?.Invoke();

        var saves = new List<SaveData>();

        for (int i = 0; i < Instance.saveables.Count; i++)
        {
            var save = new SaveData();
            save.saveData = Instance.saveables[i].Serialize();
            save.type = Instance.saveables[i].GetType();
            saves.Add(save);
        }

        var saveJson = JsonConvert.SerializeObject(saves);
        PlayerPrefs.SetString("Save", saveJson);
    }

    public static void Load()
    {
        Instance.saveables = new List<Saveable>();
        
        string loadJson = PlayerPrefs.GetString("Save");


        var load = JsonConvert.DeserializeObject<List<SaveData>>(loadJson);

        for (int i = 0; i < load.Count; i++)
        {
            var data = (Saveable)Activator.CreateInstance(load[i].type);
            data.Deserialize(load[i].saveData);
            Instance.saveables.Add(data);
        }

        OnLoaded?.Invoke();
    }

    public static void TryStoreIfNotExist(Saveable saveable)
    {
        if (saveable == null)
            return;
        if (Instance.saveables == null)
        {
            Instance.saveables = new();
        }

        if (!Instance.saveables.Contains(saveable))
            Instance.saveables.Add(saveable);
    }

    [Serializable]
    public class SaveData
    {
        public string saveData;
        public Type type;
    }
}
