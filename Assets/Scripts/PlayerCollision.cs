using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float foodValue = 5f;
    public float obstacleDamage = 35f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            HungerManager.Instance.ModifyHunger(foodValue);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            HungerManager.Instance.ModifyHunger(-obstacleDamage);
            Destroy(other.gameObject);
        }
    }
}