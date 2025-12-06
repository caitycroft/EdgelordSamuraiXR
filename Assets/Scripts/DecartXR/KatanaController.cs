using UnityEngine;
using UnityEngine.XR;
using EdgelordXR;

/// <summary>
/// Katana Controller - Tracks right controller movement and calculates slash physics
/// </summary>
public class KatanaController : MonoBehaviour
{
    [Header("XR Settings")]
    [Tooltip("Which hand holds the katana")]
    public XRNode controllerNode = XRNode.RightHand;

    [Header("Katana Physics")]
    [Tooltip("Visual representation of the katana blade")]
    public Transform bladeTransform;

    [Tooltip("Length of the katana blade in meters")]
    public float bladeLength = 0.7f;

    [Tooltip("Trail renderer for slash effects")]
    public TrailRenderer slashTrail;

    [Header("Debug")]
    public bool showDebugInfo = true;

    // Tracking data
    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 currentVelocity;
    private float currentSpeed;

    // Slash detection
    private bool isSlashing = false;
    private Vector3 slashStartPos;
    private Vector3 slashDirection;

    void Start()
    {
        if (bladeTransform == null)
        {
            Debug.LogError("Blade Transform not assigned! Please assign the katana blade object.");
        }

        if (slashTrail != null)
        {
            slashTrail.emitting = false;
        }

        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    void Update()
    {
        UpdateControllerTracking();
        UpdateVelocity();
        DetectSlash();

        if (showDebugInfo)
        {
            DebugDisplay();
        }
    }

    private void UpdateControllerTracking()
    {
        // Get controller position and rotation from XR
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);

        if (device.isValid)
        {
            Vector3 position;
            Quaternion rotation;

            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out position))
            {
                transform.localPosition = position;
            }

            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out rotation))
            {
                transform.localRotation = rotation;
            }
        }
    }

    private void UpdateVelocity()
    {
        // Calculate velocity
        currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        currentSpeed = currentVelocity.magnitude;

        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private void DetectSlash()
    {
        // Start slash detection when moving fast enough
        if (!isSlashing && currentSpeed >= 1.5f)
        {
            StartSlash();
        }
        // End slash when slowing down
        else if (isSlashing && currentSpeed < 0.5f)
        {
            EndSlash();
        }

        // Update slash direction while slashing
        if (isSlashing)
        {
            slashDirection = currentVelocity.normalized;
        }
    }

    private void StartSlash()
    {
        isSlashing = true;
        slashStartPos = transform.position;

        if (slashTrail != null)
        {
            slashTrail.emitting = true;
            slashTrail.Clear();
        }

        Debug.Log("Slash started!");
    }

    private void EndSlash()
    {
        isSlashing = false;

        if (slashTrail != null)
        {
            slashTrail.emitting = false;
        }

        Debug.Log("Slash ended!");
    }

    /// <summary>
    /// Calculate slash data when hitting an object
    /// </summary>
    public SlashData CalculateSlashData(Vector3 hitPoint, float beatOffset = 0f)
    {
        // Calculate edge alignment (Hasuji)
        Vector3 bladeForward = bladeTransform.forward; // Direction blade is pointing
        Vector3 cutDirection = currentVelocity.normalized;

        // Hasuji is perfect when blade forward aligns with cut direction
        float edgeAlignment = Vector3.Angle(bladeForward, cutDirection);

        return new SlashData(
            timingOffset: beatOffset,
            edgeAlignment: edgeAlignment,
            velocity: currentSpeed,
            cutDirection: cutDirection,
            hitPoint: hitPoint
        );
    }

    public bool IsSlashing() => isSlashing;
    public float GetCurrentSpeed() => currentSpeed;
    public Vector3 GetSlashDirection() => slashDirection;

    private void DebugDisplay()
    {
        Debug.Log($"Speed: {currentSpeed:F2} m/s | Slashing: {isSlashing}");
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // Draw velocity vector
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, currentVelocity * 0.5f);

        // Draw slash direction
        if (isSlashing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, slashDirection * bladeLength);
        }
    }
}
