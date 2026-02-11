using UnityEngine;

public class MapCam : MonoBehaviour
{
    [Header("Click Targeting")]
    public Camera raycastCamera;
    public LayerMask clickableMask = ~0;
    public Vector3 focusOffset = Vector3.zero;

    [Header("Zoom Settings")]
    public Transform camTransform;
    public float zoomInDistance = 8f;
    public float moveSmoothness = 6f;

    [Header("UI")]
    public GameObject zoomCanvas;              // drag your Canvas root here
    public float zoomCompleteThreshold = 0.05f; // how close = "finished"

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

    private bool _zoomingToTarget;
    private bool tileSelected = false;
    private bool _canvasShown;
    private RunData runData;

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

        runData = FindFirstObjectByType<RunData>();

        if (zoomCanvas != null)
            zoomCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !tileSelected)
            TryZoomToClickedObject();

        if (Input.GetKeyDown(KeyCode.Tab) && tileSelected)
            ReturnHome();

        ApplySmoothing();
        CheckZoomFinished();
    }

    void TryZoomToClickedObject()
    {
        if (raycastCamera == null) return;

        Ray ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 10000f, clickableMask, QueryTriggerInteraction.Ignore))
        {
            //clicked hex
            Transform clicked = hit.collider.transform;

            //Set hexdata to clicked hex data
            runData.hexData = clicked.GetComponentInChildren<HexCell>();

            Vector3 focusPoint = clicked.position + focusOffset;

            _targetPivotPos = focusPoint;
            _targetPivotRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            _targetCamLocalRot = camTransform.localRotation; // keep current tilt
            _targetCamLocalPos = _targetCamLocalRot * Vector3.back * zoomInDistance;

            _zoomingToTarget = true;
            tileSelected = true;
            _canvasShown = false;

            if (zoomCanvas != null)
                zoomCanvas.SetActive(false);
        }
    }

    void ReturnHome()
    {
        _targetPivotPos = _homePivotPos;
        _targetPivotRot = _homePivotRot;
        _targetCamLocalPos = _homeCamLocalPos;
        _targetCamLocalRot = _homeCamLocalRot;

        _zoomingToTarget = false;
        tileSelected = false;
        _canvasShown = false;

        //reset hexData
        runData.hexData = null;

        if (zoomCanvas != null)
            zoomCanvas.SetActive(false);
    }

    void ApplySmoothing()
    {
        float t = Time.deltaTime * moveSmoothness;

        transform.position = Vector3.Lerp(transform.position, _targetPivotPos, t);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetPivotRot, t);

        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, _targetCamLocalPos, t);
        camTransform.localRotation = Quaternion.Slerp(camTransform.localRotation, _targetCamLocalRot, t);
    }

    void CheckZoomFinished()
    {
        if (!_zoomingToTarget || _canvasShown) return;

        bool pivotClose = Vector3.Distance(transform.position, _targetPivotPos) <= zoomCompleteThreshold;
        bool camClose = Vector3.Distance(camTransform.localPosition, _targetCamLocalPos) <= zoomCompleteThreshold;

        if (pivotClose && camClose)
        {
            _canvasShown = true;
            _zoomingToTarget = false;

            if (zoomCanvas != null)
                zoomCanvas.SetActive(true);
        }
    }
}
