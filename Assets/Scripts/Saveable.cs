using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Saveable
{
    public abstract string Serialize();
    public abstract void Deserialize(string data);
    public abstract void WriteDefaults();
}
