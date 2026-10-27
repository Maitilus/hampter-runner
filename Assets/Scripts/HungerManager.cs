using UnityEngine;
using UnityEngine.UI; // Required for the Slider

public class HungerManager : MonoBehaviour
{
    public static HungerManager Instance { get; private set; }

    public Slider hungerBar;
    public float maxHunger = 100f;
    public float depletionRate = 2f;

    private float currentHunger;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        currentHunger = maxHunger;
        if (hungerBar != null)
        {
            hungerBar.maxValue = maxHunger;
            hungerBar.value = currentHunger;
        }
    }

    void Update()
    {
        if (isGameOver) return;
        ModifyHunger(-depletionRate * Time.deltaTime);
    }

    public void ModifyHunger(float amount)
    {
        if (isGameOver) return;

        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        if (hungerBar != null) hungerBar.value = currentHunger;

        if (currentHunger <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("Starved! Game Over.");
        
        TrackTile.moveSpeed = 0f; 
    }
}