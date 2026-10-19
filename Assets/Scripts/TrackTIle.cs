using UnityEngine;

public class TrackTile : MonoBehaviour
{
    public static float moveSpeed = 10f; 
    
    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        if (transform.position.z < -15f)
        {
            TrackSpawner.Instance.SpawnTile();
            Destroy(gameObject); 
        }
    }
}