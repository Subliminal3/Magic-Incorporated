using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask tileLayer;
    [SerializeField] private string highlightChildName = "Highlight";

    private Transform hoveredTileRoot;
    private GameObject hoveredHighlight;

    private Transform selectedTileRoot;
    private GameObject selectedHighlight;

    void Update()
    {
        // Tab clears the locked selection
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ClearSelection();
            // After clearing, hover will work again this frame
        }

        DetectTileHover();

        // Click locks selection (keeps highlight until Tab)
        if (Input.GetMouseButtonDown(0) && selectedTileRoot == null && hoveredTileRoot != null)
        {
            SelectHovered();
        }
    }

    void DetectTileHover()
    {
        // If a tile is selected, we don't change hover highlight
        if (selectedTileRoot != null)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayer))
        {
            Transform tileRoot = FindTileRootWithHighlight(hit.collider.transform);

            if (tileRoot != hoveredTileRoot)
            {
                ClearHover();
                SetHover(tileRoot);
            }
        }
        else
        {
            ClearHover();
        }
    }

    Transform FindTileRootWithHighlight(Transform start)
    {
        Transform t = start;
        while (t != null)
        {
            if (t.Find(highlightChildName) != null)
                return t;
            t = t.parent;
        }
        return start;
    }

    void SetHover(Transform tileRoot)
    {
        if (tileRoot == null) return;

        Transform h = tileRoot.Find(highlightChildName);
        if (h == null) return;

        h.gameObject.SetActive(true);
        hoveredTileRoot = tileRoot;
        hoveredHighlight = h.gameObject;
    }

    void ClearHover()
    {
        if (hoveredHighlight != null)
            hoveredHighlight.SetActive(false);

        hoveredHighlight = null;
        hoveredTileRoot = null;
    }

    void SelectHovered()
    {
        // Lock the currently hovered tile
        selectedTileRoot = hoveredTileRoot;
        selectedHighlight = hoveredHighlight;

        // Prevent hover system from turning it off later
        
    }

    void ClearSelection()
    {
        if (selectedHighlight != null)
            selectedHighlight.SetActive(false);

        selectedHighlight = null;
        selectedTileRoot = null;
        hoveredTileRoot = null;
        hoveredHighlight = null;
    }
}
