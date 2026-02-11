using UnityEngine;
using System.Collections.Generic;
using System;

public class HexCell : MonoBehaviour
{
    [Header("Coordinates")]
    private HexCoords coords;
    public HexCoords Coords => coords;
    [Header("Data")]
    [SerializeField] private TileDataSO _tileData;

    [Header("Battle Data")]
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

    //New class that allows setting a unit and how many of that type
    [Serializable]
    public class UnitSpawnEntry
    {
        public GameObject unitPrefab;

        //min prevents negative values
        [Min(0)] public int count;

    }
    public TileDataSO TileData => _tileData;

    public void setType(TileDataSO newType)
    {
        _tileData = newType;
       //RefreshVisuals();
    }

/*    void RefreshVisuals()
    {
        // 1. Clear old models
        foreach(Transform child in transform) Destroy(child.gameObject);

        // 2. Instantiate new model if it exists
        if (_tileData.tilePrefab != null)
        {
            Instantiate(_tileData.tilePrefab, transform);
        }
    }*/


}
