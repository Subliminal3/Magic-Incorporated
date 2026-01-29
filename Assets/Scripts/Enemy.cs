using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    public float moveSpeed = 3f;
    public float meleeRange = 1.5f;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > meleeRange)
        {
            //MoveTowardsPlayer();
            FacePlayer();
        }
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.transform.position;// - transform.position;
        lookDir.y = 0f; // keep upright

        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void HitBySpell()
    {
        
    }
}
