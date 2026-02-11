
using UnityEngine;



//[CreateAssetMenu(fileName = "NewTileData", menuName = "Hex Grid/Tile Data")]
public class TileDataSO : ScriptableObject
{
    [Header("Visuals")]
    public string tileName;
    public GameObject tilePrefab; // The 3D model for this hex

    [Header("Gameplay")]
    public BattleType battleType;

}
    