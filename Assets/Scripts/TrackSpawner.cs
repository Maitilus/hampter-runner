using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    public static TrackSpawner Instance { get; private set; }

    public GameObject tilePrefab;
    public float tileLength = 10f; 
    
    private Transform lastTile; 

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
        for (int i = 0; i < 5; i++)
        {
            SpawnTile();
        }
    }

    public void SpawnTile()
    {
        GameObject newTile = Instantiate(tilePrefab);

        if (lastTile != null)
        {
            newTile.transform.position = lastTile.position + (Vector3.forward * tileLength);
        }
        else
        {
            newTile.transform.position = Vector3.zero;
        }

        lastTile = newTile.transform;
    }
}