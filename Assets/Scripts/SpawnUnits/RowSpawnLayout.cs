using System.Collections.Generic;
using UnityEngine;

public static class RowSpawnLayout
{
    // Returns world positions inside the box bounds, arranged in rows.
    // forward: direction rows "stack" along (typically spawnObj.forward).
    public static List<Vector3> BuildRowPositions(BoxCollider box, int count, float spacing, Vector3 forward)
    {
        var positions = new List<Vector3>(count);

        Bounds b = box.bounds;

        // Flatten to XZ plane
        Vector3 f = forward; f.y = 0f;
        if (f.sqrMagnitude < 0.0001f) f = Vector3.forward;
        f.Normalize();
        Vector3 r = Vector3.Cross(Vector3.up, f).normalized;

        // Box dimensions in WORLD XZ (we’ll project onto r/f axes)
        // We approximate usable width/depth by the bounds extents on X/Z.
        // If you rotate the box, bounds stays axis-aligned, so keep the box unrotated for best results.
        float width = b.size.x;   // across r
        float depth = b.size.z;   // along f

        // How many columns fit across width?
        int cols = Mathf.Max(1, Mathf.FloorToInt((width) / spacing));
        cols = Mathf.Min(cols, count);

        // How many rows needed?
        int rows = Mathf.CeilToInt((float)count / cols);

        // If rows don't fit in depth, reduce cols to increase rows? (opposite)
        // We’ll clamp rows to what fits and recompute cols to fill.
        int maxRowsThatFit = Mathf.Max(1, Mathf.FloorToInt((depth) / spacing));
        if (rows > maxRowsThatFit)
        {
            rows = maxRowsThatFit;
            cols = Mathf.CeilToInt((float)count / rows);
        }

        Vector3 center = b.center;
        float startX = -((cols - 1) * spacing) * 0.5f;
        float startZ = -((rows - 1) * spacing) * 0.5f;

        int index = 0;

        // Distribute “half and half” naturally by filling rows evenly:
        // We compute how many per row so early rows don't get overloaded.
        int basePerRow = count / rows;
        int remainder = count % rows; // first 'remainder' rows get +1

        for (int row = 0; row < rows; row++)
        {
            int unitsThisRow = basePerRow + (row < remainder ? 1 : 0);

            // Center each row independently (so short rows are centered)
            float rowStartX = -((unitsThisRow - 1) * spacing) * 0.5f;

            float z = startZ + row * spacing;

            for (int col = 0; col < unitsThisRow; col++)
            {
                if (index >= count) break;

                float x = rowStartX + col * spacing;

                Vector3 p = center + r * x + f * z;

                // Clamp inside bounds a bit (safety)
                p.x = Mathf.Clamp(p.x, b.min.x + 0.05f, b.max.x - 0.05f);
                p.z = Mathf.Clamp(p.z, b.min.z + 0.05f, b.max.z - 0.05f);

                positions.Add(p);
                index++;
            }
        }

        return positions;
    }
}
