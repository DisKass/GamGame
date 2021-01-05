using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatsData : IPersistentData<CharacterStats>
{
    public CharacterStats.SerializableStats serializableStats;


    public float[] position;

    public CharacterStatsData() { }
    public CharacterStatsData(CharacterStats character)
    {
        Initialize(character);
    }
    public void Initialize(CharacterStats characterStats)
    {
        serializableStats = characterStats.GetSerializableStats();
        position = new float[2] { characterStats.Position.x, characterStats.Position.y };
        //Debug.Log("[CharacterStatsData] Stored position" + position[0] + " " + position[1]);
    }
}
