using UnityEngine;
using Unity.Mathematics;

[System.Serializable]
public struct HexCoords
{
    [SerializeField] private int q,r;
    public int Q => q;
    public int R => r;
    public int S => -q-r;

    public HexCoords(int q, int r)
    {
        this.q=q;
        this.r=r;
    }

    public static HexCoords operator +(HexCoords a,HexCoords b)
    {
        return new HexCoords(a.q+b.q, a.r+b.r);
    }

    public static HexCoords operator -(HexCoords a,HexCoords b)
    {
        return new HexCoords(a.q-b.q, a.r-b.r);
    }

    public static int Distance(HexCoords a, HexCoords b)
    {
        HexCoords vec = a-b;
        return (math.abs(vec.q) + math.abs(vec.q+vec.r) + math.abs(vec.r)) / 2;
    }
}