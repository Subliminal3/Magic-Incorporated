using System;
using System.Collections.Generic;
using UnityEngine;

public enum BattleType
{
    Deathmatch,
    CaptureTheFlag
}

[CreateAssetMenu(fileName = "NewTileData", menuName = "Hex Grid/Tile Data")]
public class TileDataSO : ScriptableObject
{
    [Header("Visuals")]
    public string tileName;
    public GameObject tilePrefab; // The 3D model for this hex

    [Header("Gameplay")]
    public BattleType battleType;

    [Header("Ally Units")]
    public List<UnitSpawnEntry> allyUnits = new();

    [Header("Enemy Units")]
    public List<UnitSpawnEntry> enemyUnits = new();


    //Optional to prevent duplicate spawns
    /*public int GetCount(UnitData unit)
    {
        foreach (var e in unitSpawns)
            if (e.unit == unit) return e.count;
        return 0;
    }*/

}

//New class that allows setting a unit and how many of that type
[Serializable]
public class UnitSpawnEntry
{
    public GameObject unitPrefab;

    //min prevents negative values
    [Min(0)] public int count;


}