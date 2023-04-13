using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test_HumanScript : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public GameObject startPosition;
    public GameObject goalPosition;
    private bool isStart = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        transform.position = startPosition.transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            isStart = !isStart;
            animator.SetBool("walk_bool", isStart);
        }
        if(isStart)
        {
            agent.destination = goalPosition.transform.position;
        }
    }
}
