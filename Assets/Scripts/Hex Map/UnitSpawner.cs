using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TileDataSO tileData;

    [Header("Spawn groups (parent objects that contain BoxColliders on children)")]
    public GameObject allySpawnGroup;
    public GameObject enemySpawnGroup;

    [Header("Row spacing")]
    [SerializeField] private float spacing = 2f;

    [Header("Optional NavMesh snap")]
    [SerializeField] private bool snapToNavMesh = true;
    [SerializeField] private float navMeshSampleRadius = 2f;
    [SerializeField] private int navMeshAreaMask = NavMesh.AllAreas;

    [Header("Rotation")]
    [SerializeField] private bool faceOpposingSpawn = true;

    [ContextMenu("Spawn All Now")]

    private void Start()
    {
        SpawnAll();
    }
    public void SpawnAll()
    {
        if (tileData == null) { Debug.LogError("tileData is NULL"); return; }
        if (allySpawnGroup == null) { Debug.LogError("allySpawnGroup is NULL"); return; }
        if (enemySpawnGroup == null) { Debug.LogError("enemySpawnGroup is NULL"); return; }

        SpawnTeamAcrossGroups(tileData.allyUnits, allySpawnGroup, enemySpawnGroup);
        SpawnTeamAcrossGroups(tileData.enemyUnits, enemySpawnGroup, allySpawnGroup);
    }

    private void SpawnTeamAcrossGroups(List<UnitSpawnEntry> entries, GameObject groupObj, GameObject opposingGroupObj)
    {
        if (entries == null) return;

        BoxCollider[] boxes = groupObj.GetComponentsInChildren<BoxCollider>(includeInactive: false);
        if (boxes.Length == 0)
        {
            Debug.LogError($"{groupObj.name} has no BoxColliders in children.");
            return;
        }

        int totalCount = SumCounts(entries);
        if (totalCount <= 0) return;

        // 1) build spots for each rectangle, then combine
        // We split the total across rectangles evenly: base + remainder
        int basePerBox = totalCount / boxes.Length;
        int remainder = totalCount % boxes.Length;

        var allSpots = new List<Vector3>(totalCount);

        Vector3 facingPoint = opposingGroupObj != null ? opposingGroupObj.transform.position : Vector3.zero;

        for (int i = 0; i < boxes.Length; i++)
        {
            int need = basePerBox + (i < remainder ? 1 : 0);
            if (need <= 0) continue;

            // Use the collider's parent forward as row stacking direction (or the collider's transform forward)
            Vector3 forward = boxes[i].transform.forward;

            var spots = BuildRowPositions(boxes[i], need, spacing, forward);

            // optional navmesh snap
            if (snapToNavMesh)
            {
                for (int s = 0; s < spots.Count; s++)
                {
                    if (NavMesh.SamplePosition(spots[s], out var hit, navMeshSampleRadius, navMeshAreaMask))
                        spots[s] = hit.position;
                }
            }

            allSpots.AddRange(spots);
        }

        // 2) spawn prefabs into the combined spot list
        int spotIndex = 0;

        foreach (var entry in entries)
        {
            if (entry.unitPrefab == null || entry.count <= 0) continue;

            for (int i = 0; i < entry.count; i++)
            {
                if (spotIndex >= allSpots.Count)
                {
                    Debug.LogWarning($"Not enough room in '{groupObj.name}' colliders for all units.");
                    return;
                }

                Vector3 pos = allSpots[spotIndex++];

                Quaternion rot = Quaternion.identity;
                if (faceOpposingSpawn && opposingGroupObj != null)
                {
                    Vector3 toOther = facingPoint - pos;
                    toOther.y = 0f;
                    if (toOther.sqrMagnitude > 0.001f)
                        rot = Quaternion.LookRotation(toOther.normalized);
                }

                Instantiate(entry.unitPrefab, pos, rot);
            }
        }
    }

    private int SumCounts(List<UnitSpawnEntry> entries)
    {
        int total = 0;
        foreach (var e in entries)
            if (e != null) total += Mathf.Max(0, e.count);
        return total;
    }

    // Row/column layout inside ONE BoxCollider
    // Note: uses box.bounds (axis-aligned). Keep spawn rectangles unrotated for best results.
    private List<Vector3> BuildRowPositions(BoxCollider box, int count, float spacing, Vector3 forward)
    {
        var positions = new List<Vector3>(count);

        Bounds b = box.bounds;

        Vector3 f = forward; f.y = 0f;
        if (f.sqrMagnitude < 0.0001f) f = Vector3.forward;
        f.Normalize();

        Vector3 r = Vector3.Cross(Vector3.up, f).normalized;

        float width = b.size.x;
        float depth = b.size.z;

        int cols = Mathf.Max(1, Mathf.FloorToInt(width / spacing));
        cols = Mathf.Min(cols, count);

        int rows = Mathf.CeilToInt((float)count / cols);

        int maxRows = Mathf.Max(1, Mathf.FloorToInt(depth / spacing));
        if (rows > maxRows)
        {
            rows = maxRows;
            cols = Mathf.CeilToInt((float)count / rows);
        }

        Vector3 center = b.center;
        float startZ = -((rows - 1) * spacing) * 0.5f;

        int basePerRow = count / rows;
        int remainder = count % rows;

        int placed = 0;

        for (int row = 0; row < rows; row++)
        {
            int unitsThisRow = basePerRow + (row < remainder ? 1 : 0);

            float rowStartX = -((unitsThisRow - 1) * spacing) * 0.5f;
            float z = startZ + row * spacing;

            for (int col = 0; col < unitsThisRow; col++)
            {
                if (placed >= count) break;

                float x = rowStartX + col * spacing;

                Vector3 p = center + r * x + f * z;

                p.x = Mathf.Clamp(p.x, b.min.x + 0.05f, b.max.x - 0.05f);
                p.z = Mathf.Clamp(p.z, b.min.z + 0.05f, b.max.z - 0.05f);

                positions.Add(p);
                placed++;
            }
        }

        return positions;
    }
}
