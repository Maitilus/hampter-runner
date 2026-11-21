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
                CameraShake.Instance?.Shake(0.12f, 0.08f);
                Destroy(other.gameObject);
                return;
            }

            AudioManager.Instance?.PlayCrashSounds();
            CameraShake.Instance?.Shake();
            HungerManager.Instance.ModifyHunger(-obstacleDamage);
            Destroy(other.gameObject);
        }
    }
}