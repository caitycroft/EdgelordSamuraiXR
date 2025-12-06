using UnityEngine;
using EdgelordXR;

/// <summary>
/// Sliceable Object - Objects that can be cut by the katana
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SliceableObject : MonoBehaviour
{
    [Header("Object Settings")]
    [Tooltip("Points awarded for slicing this object")]
    public int basePoints = 100;

    [Tooltip("Beat time this object should be hit on (0 = any time)")]
    public float targetBeatTime = 0f;

    [Header("Visual Feedback")]
    [Tooltip("Particle effect when sliced")]
    public GameObject sliceParticles;

    [Tooltip("Color when perfectly sliced")]
    public Color perfectSliceColor = Color.gold;

    [Tooltip("Color when poorly sliced")]
    public Color poorSliceColor = Color.gray;

    [Header("Audio")]
    public AudioClip sliceSound;

    private Rigidbody rb;
    private Renderer objectRenderer;
    private bool hasBeenSliced = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectRenderer = GetComponent<Renderer>();

        // Make sure it's a trigger for slash detection
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if hit by katana
        KatanaController katana = other.GetComponent<KatanaController>();

        if (katana != null && katana.IsSlashing() && !hasBeenSliced)
        {
            OnSliced(katana);
        }
    }

    private void OnSliced(KatanaController katana)
    {
        hasBeenSliced = true;

        // Calculate timing offset from beat
        float beatOffset = targetBeatTime > 0 ? Time.time - targetBeatTime : 0f;

        // Get slash data from katana
        SlashData slashData = katana.CalculateSlashData(transform.position, beatOffset);

        // Calculate score using Decart XR API
        SlashResult result = DecartXRAPI.Instance.CalculateSlashScore(slashData);

        // Add to combo
        if (result.totalScore >= 30f) // Minimum score to continue combo
        {
            DecartXRAPI.Instance.AddToCombo();
        }
        else
        {
            DecartXRAPI.Instance.ResetCombo();
        }

        // Apply combo multiplier
        float comboMultiplier = DecartXRAPI.Instance.GetComboMultiplier();
        int finalScore = Mathf.RoundToInt(basePoints * (result.totalScore / 100f) * comboMultiplier);

        // Visual feedback
        ShowSliceEffect(result);

        // Audio feedback
        PlaySliceSound();

        // Log results
        LogSliceResults(result, finalScore);

        // Destroy object after a delay
        Destroy(gameObject, 0.5f);
    }

    private void ShowSliceEffect(SlashResult result)
    {
        // Change color based on performance
        if (objectRenderer != null)
        {
            float t = result.totalScore / 100f;
            Color feedbackColor = Color.Lerp(poorSliceColor, perfectSliceColor, t);
            objectRenderer.material.color = feedbackColor;
        }

        // Spawn particles
        if (sliceParticles != null)
        {
            GameObject particles = Instantiate(sliceParticles, transform.position, Quaternion.identity);
            Destroy(particles, 3f);
        }

        // Make object fly away with the slash
        if (rb != null)
        {
            Vector3 slashDir = GetComponent<KatanaController>() != null
                ? GetComponent<KatanaController>().GetSlashDirection()
                : Vector3.forward;
            rb.AddForce(slashDir * 10f, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
        }
    }

    private void PlaySliceSound()
    {
        if (sliceSound != null)
        {
            AudioSource.PlayClipAtPoint(sliceSound, transform.position);
        }
    }

    private void LogSliceResults(SlashResult result, int finalScore)
    {
        Debug.Log($"=== SLICE RESULTS ===");
        Debug.Log($"Rank: {result.rank}");
        Debug.Log($"Feedback: {result.feedbackText}");
        Debug.Log($"Timing: {result.timingScore:F1}/40");
        Debug.Log($"Hasuji: {result.hasujiScore:F1}/40");
        Debug.Log($"Velocity: {result.velocityScore:F1}/20");
        Debug.Log($"Total: {result.totalScore:F1}/100");
        Debug.Log($"Combo: {DecartXRAPI.Instance.GetCurrentCombo()}x");
        Debug.Log($"Final Score: {finalScore} points");
        Debug.Log($"===================");
    }
}
