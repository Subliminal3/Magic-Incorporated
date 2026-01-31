using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitType", menuName = "Units/Unit Type")]
public class UnitData : ScriptableObject
{
    [Header("General")]
    public string unitName;
    public GameObject unitPrefab; // Optional: spawn model at runtime

    [Header("Gameplay Stats")]
    public int maxHealth = 1;
    public int attackDamage = 1;

    [Header("NavMesh Settings")]
    public float speed = 3.5f;
    public float angularSpeed = 120f; // How fast it turns
    public float acceleration = 8f;
    public float stoppingDistance = 0.5f; // How close to target before stopping
}