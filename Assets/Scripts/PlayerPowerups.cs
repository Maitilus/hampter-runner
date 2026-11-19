using UnityEngine;
using TMPro;

public class PlayerPowerups : MonoBehaviour
{
    [Header("Hamster Wheel")]
    public GameObject shieldVisual;

    [Header("Positron Core")]
    public float doublePointsDuration = 10f;
    public float doublePointsMultiplier = 2f;
    public TextMeshProUGUI doublePointsText;

    private bool hasShield = false;
    private float doublePointsTimer = 0f;

    void Start()
    {
        if (shieldVisual != null) shieldVisual.SetActive(false);
        if (doublePointsText != null) doublePointsText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (doublePointsTimer > 0f)
        {
            doublePointsTimer -= Time.deltaTime;

            if (doublePointsText != null)
            {
                doublePointsText.text = $"2X ({Mathf.CeilToInt(doublePointsTimer)}s)";
            }

            if (doublePointsTimer <= 0f)
            {
                DeactivateDoublePoints();
            }
        }
    }

    public void ActivateShield()
    {
        hasShield = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);
    }

    public bool TryConsumeShield()
    {
        if (!hasShield) return false;

        hasShield = false;
        if (shieldVisual != null) shieldVisual.SetActive(false);
        return true;
    }

    public void ActivateDoublePoints()
    {
        doublePointsTimer = doublePointsDuration;
        ScoreManager.Instance.SetBonusMultiplier(doublePointsMultiplier);

        if (doublePointsText != null)
        {
            doublePointsText.gameObject.SetActive(true);
        }
    }

    private void DeactivateDoublePoints()
    {
        doublePointsTimer = 0f;
        ScoreManager.Instance.SetBonusMultiplier(1f);

        if (doublePointsText != null)
        {
            doublePointsText.gameObject.SetActive(false);
        }
    }
}