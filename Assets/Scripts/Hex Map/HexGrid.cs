using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HexGrid : MonoBehaviour
{
    [SerializeField] private GameObject hex_tile; // MUST be the prefab asset from Project window
    [SerializeField] private int map_size;
    [SerializeField] private float outerRadius;
    [SerializeField] private float spacing;

    [ContextMenu("Generate Map")]
    void GenerateMap()
    {
        float innerRadius = outerRadius * 0.866025404f;
        int height = map_size;
        int width = map_size;

        GameObject hex_map = new GameObject("hex map");
        hex_map.transform.SetParent(this.transform, false);

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float xPos = x * (innerRadius * 2f);
                if (z % 2 != 0) xPos += innerRadius;

                float zPos = z * (outerRadius * 1.5f);

                Vector3 position = new Vector3(xPos, 0, zPos) * spacing;

                GameObject hex;

#if UNITY_EDITOR
                // Editor-safe prefab instance (keeps the link)
                hex = (GameObject)PrefabUtility.InstantiatePrefab(hex_tile, hex_map.transform);
                hex.transform.localPosition = position;
                hex.transform.localRotation = Quaternion.Euler(90, 0, 0);
#else
                // Runtime instantiate
                hex = Instantiate(hex_tile, position, Quaternion.Euler(90, 0, 0), hex_map.transform);
                hex.transform.localPosition = position;
#endif

                hex.name = $"Hex_{x}_{z}";
            }
        }
    }
}
