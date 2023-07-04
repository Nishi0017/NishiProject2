using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassedEnemyScript : MonoBehaviour
{
    private EnemyWaveManager enemyWaveManager;

    private void Start()
    {
        enemyWaveManager = transform.parent.gameObject.GetComponent<EnemyWaveManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            enemyWaveManager.defencePoint--;
        }
    }
}
