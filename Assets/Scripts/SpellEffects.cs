using UnityEngine;

public class SpellEffects : MonoBehaviour
{
    [Header("Force Push")]
    public float pushForce = 10f;
    public float pushRange = 5f;

    public void CastSpell(string spellName)
    {
        switch (spellName)
        {
            case "Push":
                ForcePush();
                break;

            case "Pull":
                ForcePull();
                break;

            case "Shield":
                Shield();
                break;
        }
    }


    //checks in a radius around the player for targetable objects with rigid body then checks which ones are in a cone area in front and applies a force
    void ForcePush()
    {
        Vector3 origin = transform.position + transform.forward * 1.5f;
        float radius = 3f;

        Collider[] hits = Physics.OverlapSphere(origin, radius);

        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb == null) continue;

            // Direction check (only in front)
            Vector3 toTarget = (rb.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toTarget);

            if (dot < 0.3f) continue; // ~70° forward cone

            rb.WakeUp();
            rb.AddForce(toTarget * pushForce, ForceMode.Impulse);
        }

    }
   

    void ForcePull()
    {
        // placeholder
    }

    void Shield()
    {
        // placeholder
    }
}
