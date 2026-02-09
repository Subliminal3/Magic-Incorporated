using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSpawner : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private TileDataSO tileData;

    [Header("Spawn areas (must have a BoxCollider)")]
    public GameObject allySpawn;
    public GameObject enemySpawn;

    [Header("Spacing")]
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private int maxTriesPerUnit = 40;

    [Header("Optional NavMesh snap")]
    [SerializeField] private bool snapToNavMesh = true;
    [SerializeField] private float navMeshSampleRadius = 2f;
    [SerializeField] private int navMeshAreaMask = NavMesh.AllAreas;

    [Header("Rotation")]
    [SerializeField] private bool faceOpposingSpawn = true;

    // caches of used points so spacing stays even
    private readonly List<Vector3> allyPlaced = new();
    private readonly List<Vector3> enemyPlaced = new();


    private void Start()
    {
        SpawnAll();
    }
    public void SpawnAll()
    {
        if (tileData == null) return;

        allyPlaced.Clear();
        enemyPlaced.Clear();

        SpawnTeam(tileData.allyUnits, allySpawn, enemySpawn, allyPlaced);
        SpawnTeam(tileData.enemyUnits, enemySpawn, allySpawn, enemyPlaced);
    }

    private void SpawnTeam(List<UnitSpawnEntry> entries, GameObject spawnObj, GameObject opposingObj, List<Vector3> placedCache)
    {
        if (entries == null || spawnObj == null) return;

        BoxCollider box = spawnObj.GetComponent<BoxCollider>();
        if (box == null)
        {
            Debug.LogError($"{spawnObj.name} needs a BoxCollider to define spawn bounds.");
            return;
        }

        Bounds bounds = box.bounds;

        foreach (var entry in entries)
        {
            if (entry.unitPrefab == null || entry.count <= 0) continue;

            for (int i = 0; i < entry.count; i++)
            {
                if (!TryGetSpawnPoint(bounds, placedCache, out var pos))
                {
                    Debug.LogWarning($"Not enough space in '{spawnObj.name}' to spawn more of {entry.unitPrefab.name}. Increase box size or lower minDistance.");
                    break;
                }

                Quaternion rot = Quaternion.identity;
                if (faceOpposingSpawn && opposingObj != null)
                {
                    Vector3 toOther = opposingObj.transform.position - pos;
                    toOther.y = 0f;
                    if (toOther.sqrMagnitude > 0.001f)
                        rot = Quaternion.LookRotation(toOther.normalized);
                }

                Instantiate(entry.unitPrefab, pos, rot);
            }
        }
    }

    private bool TryGetSpawnPoint(Bounds bounds, List<Vector3> placedCache, out Vector3 point)
    {
        for (int attempt = 0; attempt < maxTriesPerUnit; attempt++)
        {
            Vector3 candidate = RandomPointInBounds(bounds);

            // If your box collider has height, this will pick random Y too.
            // Usually you want ground level, so we snap to NavMesh or raycast.
            if (snapToNavMesh)
            {
                if (!NavMesh.SamplePosition(candidate, out var hit, navMeshSampleRadius, navMeshAreaMask))
                    continue;

                candidate = hit.position;
            }
            else
            {
                // If not snapping to NavMesh, keep candidate at the top of the box
                // or just use bounds.center.y. Adjust if needed.
                candidate.y = bounds.center.y;
            }

            if (IsFarEnough(candidate, placedCache))
            {
                placedCache.Add(candidate);
                point = candidate;
                return true;
            }
        }

        point = default;
        return false;
    }

    private Vector3 RandomPointInBounds(Bounds b)
    {
        return new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            Random.Range(b.min.z, b.max.z)
        );
    }

    private bool IsFarEnough(Vector3 candidate, List<Vector3> placedCache)
    {
        float minSqr = minDistance * minDistance;
        for (int i = 0; i < placedCache.Count; i++)
        {
            if ((placedCache[i] - candidate).sqrMagnitude < minSqr)
                return false;
        }
        return true;
    }
}
