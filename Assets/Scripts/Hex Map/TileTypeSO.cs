using UnityEngine;

[CreateAssetMenu(fileName = "NewTileType", menuName = "Hex Grid/Tile Type")]
public class TileTypeSO : ScriptableObject
{
    [Header("Visuals")]
    public string tileName;
    public GameObject prefab; // The 3D model for this hex

    [Header("Gameplay")]
    public bool isHidden = true;
    public int movementCost = 1;
    
}