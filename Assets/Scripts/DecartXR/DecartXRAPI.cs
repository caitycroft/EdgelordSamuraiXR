using UnityEngine;
using System;

namespace EdgelordXR
{
    /// <summary>
    /// Decart XR API - Core scoring and feedback system for Edgelord Samurai mechanics
    /// Calculates slash quality based on Timing, Hasuji (edge alignment), and Velocity
    /// </summary>
    public class DecartXRAPI : MonoBehaviour
    {
        #region Singleton
        public static DecartXRAPI Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Scoring Constants
        [Header("Scoring Thresholds")]
        [Tooltip("Perfect timing window in seconds (±)")]
        public float perfectTimingWindow = 0.1f;

        [Tooltip("Good timing window in seconds (±)")]
        public float goodTimingWindow = 0.25f;

        [Tooltip("Perfect hasuji angle threshold in degrees")]
        public float perfectHasujiAngle = 15f;

        [Tooltip("Good hasuji angle threshold in degrees")]
        public float goodHasujiAngle = 35f;

        [Tooltip("Minimum velocity for a valid cut (m/s)")]
        public float minCutVelocity = 2f;

        [Tooltip("Perfect velocity threshold (m/s)")]
        public float perfectVelocity = 5f;
        #endregion

        #region Score Calculation
        /// <summary>
        /// Calculate slash score (0-100) based on three metrics
        /// </summary>
        public SlashResult CalculateSlashScore(SlashData data)
        {
            SlashResult result = new SlashResult();

            // 1. TIMING SCORE (0-40 points)
            result.timingScore = CalculateTimingScore(data.timingOffset);

            // 2. HASUJI SCORE (0-40 points) - Edge alignment
            result.hasujiScore = CalculateHasujiScore(data.edgeAlignment);

            // 3. VELOCITY SCORE (0-20 points)
            result.velocityScore = CalculateVelocityScore(data.velocity);

            // Total score
            result.totalScore = result.timingScore + result.hasujiScore + result.velocityScore;

            // Determine rank
            result.rank = DetermineRank(result.totalScore);

            // Japanese feedback text
            result.feedbackText = GetJapaneseFeedback(result.rank);

            return result;
        }

        private float CalculateTimingScore(float timingOffset)
        {
            float absOffset = Mathf.Abs(timingOffset);

            if (absOffset <= perfectTimingWindow)
            {
                return 40f; // PERFECT
            }
            else if (absOffset <= goodTimingWindow)
            {
                // Linear interpolation between perfect and good
                float t = (absOffset - perfectTimingWindow) / (goodTimingWindow - perfectTimingWindow);
                return Mathf.Lerp(40f, 20f, t);
            }
            else
            {
                // Diminishing returns after good window
                return Mathf.Max(0f, 20f - (absOffset - goodTimingWindow) * 10f);
            }
        }

        private float CalculateHasujiScore(float edgeAlignment)
        {
            if (edgeAlignment <= perfectHasujiAngle)
            {
                return 40f; // PERFECT HASUJI
            }
            else if (edgeAlignment <= goodHasujiAngle)
            {
                float t = (edgeAlignment - perfectHasujiAngle) / (goodHasujiAngle - perfectHasujiAngle);
                return Mathf.Lerp(40f, 20f, t);
            }
            else
            {
                return Mathf.Max(0f, 20f - (edgeAlignment - goodHasujiAngle) * 0.5f);
            }
        }

        private float CalculateVelocityScore(float velocity)
        {
            if (velocity < minCutVelocity)
            {
                return 0f; // Too slow to cut
            }
            else if (velocity >= perfectVelocity)
            {
                return 20f; // PERFECT VELOCITY
            }
            else
            {
                // Linear interpolation between min and perfect
                float t = (velocity - minCutVelocity) / (perfectVelocity - minCutVelocity);
                return Mathf.Lerp(10f, 20f, t);
            }
        }

        private SlashRank DetermineRank(float totalScore)
        {
            if (totalScore >= 95f) return SlashRank.EDGELORD;
            if (totalScore >= 85f) return SlashRank.MASTER;
            if (totalScore >= 70f) return SlashRank.SAMURAI;
            if (totalScore >= 50f) return SlashRank.WARRIOR;
            if (totalScore >= 30f) return SlashRank.NOVICE;
            return SlashRank.MISS;
        }

        private string GetJapaneseFeedback(SlashRank rank)
        {
            switch (rank)
            {
                case SlashRank.EDGELORD:
                    return "絶対領域！\nABSOLUTE DOMAIN!";
                case SlashRank.MASTER:
                    return "神業！\nDIVINE SKILL!";
                case SlashRank.SAMURAI:
                    return "見事！\nSPLENDID!";
                case SlashRank.WARRIOR:
                    return "良い！\nGOOD!";
                case SlashRank.NOVICE:
                    return "頑張れ！\nKEEP TRYING!";
                case SlashRank.MISS:
                    return "残念...\nMISSED...";
                default:
                    return "";
            }
        }
        #endregion

        #region Combo System
        private int currentCombo = 0;
        private float comboTimer = 0f;
        private const float COMBO_TIMEOUT = 3f;

        public int GetCurrentCombo() => currentCombo;
        public float GetComboMultiplier() => GetComboMultiplier(currentCombo);

        public float GetComboMultiplier(int combo)
        {
            if (combo >= 20) return 4f; // EDGELORD MODE
            if (combo >= 10) return 2f;
            if (combo >= 5) return 1.5f;
            return 1f;
        }

        public void AddToCombo()
        {
            currentCombo++;
            comboTimer = COMBO_TIMEOUT;
            Debug.Log($"COMBO: {currentCombo}x (Multiplier: {GetComboMultiplier()}x)");
        }

        public void ResetCombo()
        {
            if (currentCombo > 0)
            {
                Debug.Log($"Combo broken at {currentCombo}x");
            }
            currentCombo = 0;
            comboTimer = 0f;
        }

        private void Update()
        {
            if (currentCombo > 0)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0f)
                {
                    ResetCombo();
                }
            }
        }
        #endregion
    }

    #region Data Structures
    [Serializable]
    public struct SlashData
    {
        public float timingOffset;      // How far from the beat (in seconds)
        public float edgeAlignment;     // Angle difference from perfect cut (in degrees)
        public float velocity;          // Swing speed (m/s)
        public Vector3 cutDirection;    // Direction of the cut
        public Vector3 hitPoint;        // Where the object was hit

        public SlashData(float timing, float hasuji, float vel, Vector3 cutDir, Vector3 hit)
        {
            timingOffset = timing;
            edgeAlignment = hasuji;
            velocity = vel;
            cutDirection = cutDir;
            hitPoint = hit;
        }
    }

    [Serializable]
    public struct SlashResult
    {
        public float timingScore;       // 0-40 points
        public float hasujiScore;       // 0-40 points
        public float velocityScore;     // 0-20 points
        public float totalScore;        // 0-100 points
        public SlashRank rank;
        public string feedbackText;
    }

    public enum SlashRank
    {
        MISS,
        NOVICE,
        WARRIOR,
        SAMURAI,
        MASTER,
        EDGELORD
    }
    #endregion
}
