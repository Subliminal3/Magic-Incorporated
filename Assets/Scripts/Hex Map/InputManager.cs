using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask tileLayer;

    [Header("Materials")]
    [SerializeField] private Material highlightMaterial;

    private Renderer lastRenderer;
    private Material originalMaterial;

    void Update()
    {
        DetectTile();
    }

    void DetectTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tileLayer))
        {
            // IMPORTANT: collider might be on a child
            Renderer currentRenderer = hit.collider.GetComponentInParent<Renderer>();

            if (currentRenderer != lastRenderer)
            {
                ClearHighlight();
                Highlight(currentRenderer);
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    void Highlight(Renderer rend)
    {
        if (rend == null || highlightMaterial == null) return;

        lastRenderer = rend;

        // Cache original material
        originalMaterial = rend.material;

        // Swap to highlight material
        rend.material = highlightMaterial;
    }

    void ClearHighlight()
    {
        if (lastRenderer == null) return;

        // Restore original material
        lastRenderer.material = originalMaterial;

        lastRenderer = null;
        originalMaterial = null;
    }
}
