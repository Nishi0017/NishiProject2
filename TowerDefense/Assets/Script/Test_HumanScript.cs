using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test_HumanScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public Transform goalPosition;
    private bool isStart = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("walk_bool", true);
        agent.destination = goalPosition.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "goal")
        {
            animator.SetBool("walk_bool", false);
            Destroy(gameObject);
        }
    }
}
