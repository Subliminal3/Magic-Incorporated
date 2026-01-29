using UnityEngine;

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
        Vector3 origin = transform.position + transform.forward * .5f;
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
}
