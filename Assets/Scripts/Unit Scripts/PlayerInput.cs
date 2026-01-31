using UnityEngine;
public class PlayerInputDriver : MonoBehaviour
{
    [SerializeField] private UnitController myUnit;
    [SerializeField] private LayerMask groundLayer; // Set this in Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                myUnit.MoveTo(hit.point);
            }
        }
    }
}