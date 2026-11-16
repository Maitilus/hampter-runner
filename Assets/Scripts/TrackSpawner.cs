using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    public static TrackSpawner Instance { get; private set; }

    [Header("Tile Prefabs")]
    public GameObject emptyTilePrefab;
    public GameObject[] obstacleTilePrefabs;

    [Header("Track Settings")]
    public float tileLength = 10f;
    public Vector3 tileRotation = new Vector3(0f, 90f, 0f);
    public int initialTileCount = 30;
    public int safeStartTiles = 3;

    [Header("Speed Progression")]
    public float startSpeed = 10f;
    public float maxSpeed = 30f;
    [Tooltip("0.05 = 5% faster per minute")]
    public float speedIncreasePerMinute = 0.05f;

    private Transform lastTile;
    private int spawnedCount = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Reset speed cleanly at the start of every run
        TrackTile.moveSpeed = startSpeed;

        for (int i = 0; i < initialTileCount; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        // Only accelerate while the game is actively running (not on Game Over)
        if (TrackTile.moveSpeed > 0f && TrackTile.moveSpeed < maxSpeed)
        {
            float growthPerSecond = (startSpeed * speedIncreasePerMinute) / 60f;
            TrackTile.moveSpeed = Mathf.Min(maxSpeed, TrackTile.moveSpeed + growthPerSecond * Time.deltaTime);
        }
    }

    public void SpawnTile()
    {
        GameObject prefabToSpawn = ChooseTilePrefab();

        Quaternion spawnRot = Quaternion.Euler(tileRotation);
        Vector3 spawnPos = (lastTile != null)
            ? lastTile.position + (Vector3.forward * tileLength)
            : Vector3.zero;

        GameObject newTile = Instantiate(prefabToSpawn, spawnPos, spawnRot);
        lastTile = newTile.transform;
        spawnedCount++;
    }

    private GameObject ChooseTilePrefab()
    {
        if (spawnedCount < safeStartTiles || obstacleTilePrefabs == null || obstacleTilePrefabs.Length == 0)
        {
            return emptyTilePrefab;
        }

        int randomIndex = Random.Range(0, obstacleTilePrefabs.Length);
        return obstacleTilePrefabs[randomIndex];
    }
}