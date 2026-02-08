using UnityEngine;

public class MapCam : MonoBehaviour
{
    [Header("Click Targeting")]
    public Camera raycastCamera;
    public LayerMask clickableMask = ~0;     // set to only the stuff you want clickable
    public bool useRootObject = false;        // if you click a child collider, zoom to the root
    public Vector3 focusOffset = Vector3.zero; // e.g. (0,1,0) to focus above the object

    [Header("Zoom Settings")]
    public Transform camTransform;
    public float zoomInDistance = 8f;
    public float zoomPitch = 60f;
    public float moveSmoothness = 6f;

    // Home state
    private Vector3 _homePivotPos;
    private Quaternion _homePivotRot;
    private Vector3 _homeCamLocalPos;
    private Quaternion _homeCamLocalRot;

    // Targets
    private Vector3 _targetPivotPos;
    private Quaternion _targetPivotRot;
    private Vector3 _targetCamLocalPos;
    private Quaternion _targetCamLocalRot;

    void Start()
    {
        if (raycastCamera == null) raycastCamera = Camera.main;

        _homePivotPos = transform.position;
        _homePivotRot = transform.rotation;

        _homeCamLocalPos = camTransform.localPosition;
        _homeCamLocalRot = camTransform.localRotation;

        _targetPivotPos = _homePivotPos;
        _targetPivotRot = _homePivotRot;
        _targetCamLocalPos = _homeCamLocalPos;
        _targetCamLocalRot = _homeCamLocalRot;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryZoomToClickedObject();

        if (Input.GetKeyDown(KeyCode.Tab))
            ReturnHome();

        ApplySmoothing();
    }

    void TryZoomToClickedObject()
    {
        if (raycastCamera == null) return;

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, clickableMask, QueryTriggerInteraction.Ignore))
        {
            Transform clicked = hit.collider.transform; // IMPORTANT

            //if (useRootObject) clicked = clicked.root;

            Vector3 focusPoint = clicked.position + focusOffset;

            _targetPivotPos = focusPoint;
            _targetPivotRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            _targetCamLocalRot = camTransform.localRotation; // no tilt change
            _targetCamLocalPos = _targetCamLocalRot * Vector3.back * zoomInDistance;
        }
    }


    void ReturnHome()
    {
        _targetPivotPos = _homePivotPos;
        _targetPivotRot = _homePivotRot;
        _targetCamLocalPos = _homeCamLocalPos;
        _targetCamLocalRot = _homeCamLocalRot;
    }

    void ApplySmoothing()
    {
        float t = Time.deltaTime * moveSmoothness;

        transform.position = Vector3.Lerp(transform.position, _targetPivotPos, t);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetPivotRot, t);

        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, _targetCamLocalPos, t);
        camTransform.localRotation = Quaternion.Slerp(camTransform.localRotation, _targetCamLocalRot, t);
    }
}
