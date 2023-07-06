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

    /// <summary>
    /// “G‚ªƒvƒŒƒCƒ„[(–h‰q{İ)‚ÌUŒ‚‚É‚æ‚Á‚Ä“|‚³‚ê‚½Û‚ÉŒÄ‚Ño‚·•Ï”
    /// </summary>
    public void EnemyDefeated()
    {
        GameManager.Instance.GetMoney(enemyDate.reward);
    }

    private void OnDestroy()
    {
        UISystemManager uISystemManager = GameObject.FindWithTag("UISystemManager").GetComponent<UISystemManager>();
        uISystemManager.UpdateEnemyTotal();
    }
}
