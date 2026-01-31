using UnityEngine;

public class MapCam : MonoBehaviour
{
    [Header("Rotation & Pitch")]
    public float rotationSpeed = 100f;
    public float pitchSpeed = 80f;
    public float smoothness = 5f;
    public float minPitch = 30f; // Don't look too flat
    public float maxPitch = 80f; // Don't look straight down

    [Header("Zoom")]
    public Transform camTransform; 
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 25f;

    private float _targetYaw;
    private float _targetPitch;
    private float _targetZoom;

    void Start()
    {
        _targetYaw = transform.eulerAngles.y;
        _targetPitch = camTransform.localEulerAngles.x;
        _targetZoom = camTransform.localPosition.magnitude;
    }

    void Update()
    {
        HandleInputs();
        ApplySmoothing();
    }

    void HandleInputs()
    {
        // Orbit and Pitch when holding Right Mouse Button
        if (Input.GetMouseButton(1))
        {
            _targetYaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            
            // Invert the Mouse Y for natural "pull to tilt" feeling
            _targetPitch -= Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;
            _targetPitch = Mathf.Clamp(_targetPitch, minPitch, maxPitch);

            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // Zoom logic
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _targetZoom -= scroll * zoomSpeed;
        _targetZoom = Mathf.Clamp(_targetZoom, minZoom, maxZoom);
    }

    void ApplySmoothing()
    {
        // 1. Rotate the Pivot (Left/Right)
        float currentYaw = Mathf.LerpAngle(transform.eulerAngles.y, _targetYaw, Time.deltaTime * smoothness);
        transform.rotation = Quaternion.Euler(0, currentYaw, 0);

        // 2. Rotate the Camera itself (Up/Down)
        float currentPitch = Mathf.LerpAngle(camTransform.localEulerAngles.x, _targetPitch, Time.deltaTime * smoothness);
        camTransform.localRotation = Quaternion.Euler(currentPitch, 0, 0);

        // 3. Move the Camera (Orbit around pivot)
        // Smoothly interpolate the zoom distance separately
        float currentDist = Vector3.Distance(transform.position, camTransform.position);
        float targetDist = Mathf.Lerp(currentDist, _targetZoom, Time.deltaTime * smoothness);

        // Calculate the orbit position relative to the pivot:
        // Take the rotation from Step 2 (Pitch) and extend backward by the zoom distance.
        // Since we are setting localPosition relative to the Yaw-rotated parent, 
        // we only need to apply the local Pitch rotation here.
        camTransform.localPosition = camTransform.localRotation * Vector3.back * targetDist;
    }
}
