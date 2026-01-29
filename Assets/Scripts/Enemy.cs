using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private PlayerStats playerStats;
    private NavMeshAgent enemy;
    private bool isAttacking = false;

    private event Action OnHit;
    [SerializeField] float meleeRange = 4f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float attackDamage = 5f;
    [SerializeField] float attackBuffer = 1.5f;


    private void Start()
    {

        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        enemy.stoppingDistance = meleeRange;
        enemy.speed = moveSpeed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            Debug.Log("No player found"); 
            return;
        }

        //Vector from enemy to player
        Vector3 distToPlayer = player.transform.position - transform.position;

        //Square for optimization
        float sqrDistance = distToPlayer.sqrMagnitude;
        
        MoveTowardsPlayer();

        //if enemy is in melee range
        if (sqrDistance <= meleeRange * meleeRange && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackSequence());
        }
        
    }

    IEnumerator AttackSequence()
    {
        while (isAttacking)
        {
            //if the player is too far stops the damage
            if ((player.transform.position - transform.position).sqrMagnitude > meleeRange * meleeRange)
            {
                isAttacking = false;
                yield break; // stop the coroutine if player moves out of range
            }

            playerStats.currentHP -= attackDamage;

            yield return new WaitForSeconds(attackBuffer);
        }
        
    }

    void MoveTowardsPlayer()
    {
        enemy.SetDestination(player.transform.position);
    }

    public void HitBySpell()
    {
        OnHit?.Invoke();
    }
}
