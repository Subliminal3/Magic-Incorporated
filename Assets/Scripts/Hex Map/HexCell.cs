using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    [Header("Coordinates")]
    private HexCoords coords;
    public HexCoords Coords => coords;
    [Header("Data")]
    [SerializeField] private TileTypeSO _tileType;
    public TileTypeSO TileType => _tileType;

    public void setType(TileTypeSO newType)
    {
        _tileType=newType;
        RefreshVisuals();
    }

    void RefreshVisuals()
    {
        // 1. Clear old models
        foreach(Transform child in transform) Destroy(child.gameObject);

        // 2. Instantiate new model if it exists
        if (_tileType.prefab != null)
        {
            Instantiate(_tileType.prefab, transform);
        }
    }
}
