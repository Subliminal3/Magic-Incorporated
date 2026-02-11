using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHP = 100;
    public float currentHP;


    private void Start()
    {
        currentHP = maxHP;
    }
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }
}
