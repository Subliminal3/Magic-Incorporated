using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent enemy;

    public float moveSpeed = 3f;
    public event Action OnHit;
    //public float meleeRange = 1.5f;


    private void Start()
    {

        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            Debug.Log("No player found"); 
            return;
        }

        
        MoveTowardsPlayer();
        
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
