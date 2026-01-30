using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpellEffects : MonoBehaviour
{
    [Header("Force Push")]
    public float pushForce = 10f;
    public float pushRange = 5f;

    [Header("VFX")]
    public GameObject pushEffect;

    // Scale multiplier for the VFX, editable in the Inspector
    [Tooltip("Multiplier for the visual size of the effect")]
    public float pushEffectScale = 1f;

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
        //range for detection sphere
        Vector3 origin = transform.position + transform.forward * .5f;
        float radius = 3f;

        //cast detection sphere and add the hit objects into an array
        Collider[] hits = Physics.OverlapSphere(origin, radius);

        //for each one in the array with an rb do this
        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.attachedRigidbody;
            if (rb == null) continue;

            // Direction check (only in front)
            Vector3 toTarget = (rb.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, toTarget);

            if (dot < 0.3f) continue; // ~70° forward cone

            Debug.Log("Pushing");
            rb.isKinematic = false;   // Allow physics
            rb.WakeUp();

            //Disable nav mesh to apply force
            NavMeshAgent agent = rb.GetComponentInParent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = false;
            }

            //Add force
            rb.AddForce(toTarget * pushForce, ForceMode.Impulse);

            agent.velocity = rb.linearVelocity;

            // Start coroutine to restore kinematic
            StartCoroutine(ReturnToKinematic(rb, agent));

            // GET ENEMY SCRIPT AND CALL FUNCTION
            Enemy enemy = rb.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitBySpell();
            }
        }

        // play effect

        // Spawn effect
        if (pushEffect != null)
        {

            GameObject effect = Instantiate(pushEffect, origin, Quaternion.LookRotation(transform.forward));

            // Scale using the editable multiplier
            effect.transform.localScale = Vector3.one * pushEffectScale;

            Destroy(effect, 3f);

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();
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

    IEnumerator ReturnToKinematic(Rigidbody rb, NavMeshAgent agent)
    {
        yield return new WaitForSeconds(5f);

        // Wait until the Rigidbody slows down
        while (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            yield return null; // wait 1 frame
        }

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        agent.enabled = true;

        
    }
}
