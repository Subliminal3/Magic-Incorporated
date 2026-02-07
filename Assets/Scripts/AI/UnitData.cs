using Unity.VisualScripting;
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
    public float attackRange = 4;
    public float attackSpeed = 1.5f;

    [Header("NavMesh Settings")]
    public float speed = 8f;
    public float angularSpeed = 720f; // How fast it turns
    public float acceleration = 12f;
    public float stoppingDistance = 4f; // How close to target before stopping
    public float detectionRange = 10f;

    
}