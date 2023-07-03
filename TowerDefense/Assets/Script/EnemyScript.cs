using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public EnemyDate enemyDate;

    [SerializeField] private int level = 0;

    private NavMeshAgent agent;
    private Animator animator;


    void Start()
    {
        //Animator
        animator = GetComponent<Animator>();
        animator.SetBool("move_bool", true);

    }

    public void InputEnemyInformation(Transform _goalPos, int _level)
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyDate.speed;
        agent.destination = _goalPos.position;

        //agent.SetDestination(_goalPos.position);
    
        level = _level;
    }
}
