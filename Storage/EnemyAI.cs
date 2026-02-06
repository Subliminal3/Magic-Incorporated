using UnityEngine;

public class EnemyAI : AIController
{
    [Header("Enemy")]
    public Transform EndTarget;          // fallback target when no enemy found which should be player
    public LayerMask allyMask;       // set to ally layer in Inspector

    protected override void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            allyMask
        );

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
        else
        {
            target = EndTarget;
        }

        Debug.Log("Enemy finding target");
    }
}
