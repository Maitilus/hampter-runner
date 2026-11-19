using UnityEngine;

[RequireComponent(typeof(PlayerPowerups))]
public class PlayerCollision : MonoBehaviour
{
    public float foodValue = 20f;
    public float obstacleDamage = 30f;

    private PlayerPowerups powerups;

    void Awake()
    {
        powerups = GetComponent<PlayerPowerups>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            HungerManager.Instance.ModifyHunger(foodValue);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ShieldPowerup"))
        {
            powerups.ActivateShield();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("DoublePointsPowerup"))
        {
            powerups.ActivateDoublePoints();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            if (powerups.TryConsumeShield())
            {
                Destroy(other.gameObject);
                return;
            }

            HungerManager.Instance.ModifyHunger(-obstacleDamage);
            Destroy(other.gameObject);
        }
    }
}