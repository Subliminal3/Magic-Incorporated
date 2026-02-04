using UnityEngine;

public class AllyAI : AIController
{
    [Header("Ally")]
    public Transform EndTarget;          // fallback target when no enemy found
    public LayerMask enemyMask;       // set to Enemy layer in Inspector

    protected override void FindTarget()
    {

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            detectionRadius,
            enemyMask
        );

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
        else
        {
            target = EndTarget;
        }

        Debug.Log("Ally Finding new target");
    }
}
