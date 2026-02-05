using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Units/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("General")]
    public string unitName;
    public GameObject unitPrefab; // Optional: spawn model at runtime
    //public AIStateMachine stateMachine;

    [Header("Gameplay Stats")]
    public int maxHealth = 1;
    public int attackDamage = 1;
    public float attackRange = 2;
    public float attackCooldown = 1.5f;

    [Header("NavMesh Settings")]
    public float speed = 10f;
    public float angularSpeed = 120f; // How fast it turns
    public float acceleration = 8f;
    public float stoppingDistance = 1f; // How close to target before stopping
    public float detectionRange = 10f;
}