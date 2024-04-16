using Newtonsoft.Json;

public class LevelSaver : Saveable
{ 
    public LevelSave save;
    
    public override void Deserialize(string data)
    {
        save = JsonConvert.DeserializeObject<LevelSave>(data);
    }

    public override string Serialize()
    {
        return JsonConvert.SerializeObject(save);
    }

    public override void WriteDefaults()
    {
        save = new LevelSave
        {
            currentLevel = 0
        };
    }

    [System.Serializable]
    public class LevelSave
    {
        public int currentLevel;
    }
}
