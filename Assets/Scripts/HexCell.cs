using UnityEngine;
using System.Collections.Generic;

public class HexCell : MonoBehaviour
{
    private HexCoords coords;
    public HexCoords Coords => coords;
    [SerializeField] float outradius;
    [SerializeField] float inradius;
}
