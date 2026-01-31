using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private LayerMask tileLayer; // Set this in the Inspector to your "Tiles" layer
    private GameObject lastHighlightedTile;

    void Update()
    {
        DetectTile();
    }

    void DetectTile()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, tileLayer))
        {
            GameObject currentTile = hit.collider.gameObject;

            // Only act if we've moved to a NEW tile
            if (currentTile != lastHighlightedTile)
            {
                ClearHighlight();
                HighlightTile(currentTile);
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void HighlightTile(GameObject tile)
    {
        lastHighlightedTile = tile;
        // Example: Change color or enable a highlight child object
        tile.GetComponent<Renderer>().material.color = Color.yellow;
    }

    void ClearHighlight()
    {
        if (lastHighlightedTile != null)
        {
            lastHighlightedTile.GetComponent<Renderer>().material.color = Color.white;
            lastHighlightedTile = null;
        }
    }
}
