using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public float scoreMultiplier = 1f;

    private float bonusMultiplier = 1f;
    private float runScore;
    private int totalCurrency;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        totalCurrency = PlayerPrefs.GetInt("TotalCurrency", 0);
    }

    void Update()
    {
        if (TrackTile.moveSpeed > 0)
        {
            runScore += TrackTile.moveSpeed * scoreMultiplier * bonusMultiplier * Time.deltaTime;

            if (scoreText != null)
            {
                scoreText.text = Mathf.FloorToInt(runScore).ToString();
            }
        }
    }

    public void AddPoints(int points)
    {
        runScore += points * bonusMultiplier;
    }

    public void SetBonusMultiplier(float multiplier)
    {
        bonusMultiplier = multiplier;
    }

    public void SaveRunScore()
    {
        int finalRunScore = Mathf.FloorToInt(runScore);
        totalCurrency += finalRunScore;

        PlayerPrefs.SetInt("TotalCurrency", totalCurrency);
        PlayerPrefs.Save();
    }
}