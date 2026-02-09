using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    [Header("Coordinates")]
    private HexCoords coords;
    public HexCoords Coords => coords;
    [Header("Data")]
    [SerializeField] private TileDataSO _tileType;
    public TileDataSO TileType => _tileType;

    public void setType(TileDataSO newType)
    {
        _tileType=newType;
        RefreshVisuals();
    }

    void RefreshVisuals()
    {
        // 1. Clear old models
        foreach(Transform child in transform) Destroy(child.gameObject);

        // 2. Instantiate new model if it exists
        if (_tileType.tilePrefab != null)
        {
            Instantiate(_tileType.tilePrefab, transform);
        }
    }
}
