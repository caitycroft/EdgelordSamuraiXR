using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EdgelordXR;
using System.Collections;

/// <summary>
/// Samurai Feedback UI - Displays performance feedback in Japanese samurai style
/// </summary>
public class SamuraiFeedbackUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Text for displaying slash rank and feedback")]
    public TextMeshProUGUI feedbackText;

    [Tooltip("Text for displaying score")]
    public TextMeshProUGUI scoreText;

    [Tooltip("Text for displaying combo")]
    public TextMeshProUGUI comboText;

    [Tooltip("Image for rank icon/badge")]
    public Image rankImage;

    [Header("Feedback Animation")]
    [Tooltip("How long feedback stays on screen")]
    public float feedbackDuration = 2f;

    [Tooltip("Feedback text animation curve")]
    public AnimationCurve feedbackCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Colors")]
    public Color edgelordColor = new Color(1f, 0.84f, 0f); // Gold
    public Color masterColor = new Color(1f, 0.27f, 0f); // Red-Orange
    public Color samuraiColor = new Color(0.8f, 0.2f, 0.2f); // Red
    public Color warriorColor = new Color(0.4f, 0.6f, 1f); // Blue
    public Color noviceColor = Color.white;
    public Color missColor = Color.gray;

    private int totalScore = 0;

    void Start()
    {
        if (feedbackText != null) feedbackText.text = "";
        if (scoreText != null) scoreText.text = "SCORE: 0";
        if (comboText != null) comboText.text = "";
    }

    void Update()
    {
        UpdateComboDisplay();
    }

    /// <summary>
    /// Show slash feedback on screen
    /// </summary>
    public void ShowSlashFeedback(SlashResult result, int points)
    {
        totalScore += points;

        // Update score
        if (scoreText != null)
        {
            scoreText.text = $"SCORE: {totalScore}";
        }

        // Show feedback animation
        StartCoroutine(AnimateFeedback(result, points));
    }

    private IEnumerator AnimateFeedback(SlashResult result, int points)
    {
        if (feedbackText == null) yield break;

        // Set color based on rank
        Color rankColor = GetRankColor(result.rank);
        feedbackText.color = rankColor;

        // Set text
        string displayText = $"{result.feedbackText}\n";
        displayText += $"+{points} pts\n";
        displayText += $"[{result.totalScore:F0}/100]";

        feedbackText.text = displayText;

        // Animate
        float elapsed = 0f;
        Vector3 originalScale = feedbackText.transform.localScale;

        while (elapsed < feedbackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / feedbackDuration;

            // Scale animation
            float scale = feedbackCurve.Evaluate(t);
            feedbackText.transform.localScale = originalScale * (1f + scale * 0.5f);

            // Fade out at end
            if (t > 0.7f)
            {
                float fadeT = (t - 0.7f) / 0.3f;
                Color color = rankColor;
                color.a = 1f - fadeT;
                feedbackText.color = color;
            }

            yield return null;
        }

        // Reset
        feedbackText.text = "";
        feedbackText.transform.localScale = originalScale;
        Color resetColor = rankColor;
        resetColor.a = 1f;
        feedbackText.color = resetColor;
    }

    private void UpdateComboDisplay()
    {
        if (comboText == null) return;

        int combo = DecartXRAPI.Instance.GetCurrentCombo();

        if (combo > 0)
        {
            float multiplier = DecartXRAPI.Instance.GetComboMultiplier();
            comboText.text = $"COMBO: {combo}x\n";
            comboText.text += $"MULTIPLIER: {multiplier:F1}x";

            // Edgelord mode special effect
            if (combo >= 20)
            {
                comboText.text += "\n<color=#FFD700>★ EDGELORD MODE ★</color>";
                comboText.fontSize = 48;
            }
            else
            {
                comboText.fontSize = 36;
            }

            // Pulse effect
            float pulse = 1f + Mathf.Sin(Time.time * 5f) * 0.1f;
            comboText.transform.localScale = Vector3.one * pulse;
        }
        else
        {
            comboText.text = "";
        }
    }

    private Color GetRankColor(SlashRank rank)
    {
        switch (rank)
        {
            case SlashRank.EDGELORD: return edgelordColor;
            case SlashRank.MASTER: return masterColor;
            case SlashRank.SAMURAI: return samuraiColor;
            case SlashRank.WARRIOR: return warriorColor;
            case SlashRank.NOVICE: return noviceColor;
            case SlashRank.MISS: return missColor;
            default: return Color.white;
        }
    }

    public int GetTotalScore() => totalScore;
    public void ResetScore() => totalScore = 0;
}
