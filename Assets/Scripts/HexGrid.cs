using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;


public class HexGrid : MonoBehaviour
{
    [SerializeField] GameObject hex_tile;
    Dictionary<HexCoords, HexCell> hex_grid;
    [SerializeField] int map_size;
    [SerializeField] float outerRadius;
    [SerializeField] float spacing;

    [ContextMenu("Generate Map")]
    void GenerateMap()
    {
        // Inner radius math: r = R * cos(30 degrees)
        float innerRadius = outerRadius * 0.866025404f;
        int height = map_size;
        int width = map_size;
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 position;

                // Horizontal distance between hex centers is 2 * innerRadius
                // Every odd row (z % 2 != 0) is offset by 1 innerRadius
                float xPos = x * (innerRadius * 2f);
                if (z % 2 != 0)
                {
                    xPos += innerRadius;
                }

                // Vertical distance between rows is 1.5 * outerRadius
                float zPos = z * (outerRadius * 1.5f);

                position = new Vector3(xPos, 0, zPos) * spacing;

                GameObject hex = Instantiate(hex_tile, position, Quaternion.Euler(90,0,0));
                hex.transform.SetParent(transform);
                hex.name = $"Hex_{x}_{z}";
            }
        }
    }
}

