using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats health = other.GetComponent<PlayerStats>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}