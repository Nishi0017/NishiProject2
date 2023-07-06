using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpScript : MonoBehaviour
{
    private EnemyDate enemyDate;

    private int currentHp = 0;
    [SerializeField] private Slider slider;

    [SerializeField] private int enemyDef = 0;


    private void Start()
    {
        enemyDate = GetComponent<EnemyScript>().enemyDate;
        currentHp = enemyDate.health;

        slider.value = 1;
    }

    public void Damage(int _damage)
    {
        currentHp -= _damage;
        if(currentHp < 1)
        {
            GetComponent<EnemyScript>().EnemyDefeated();
            Destroy(gameObject);
        }
        slider.value = (float)currentHp / (float)enemyDate.health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AllyAttack"))
        {

            Damage(other.gameObject.GetComponent<BulletScript>().damage);
            Destroy(other.gameObject);
        }
    }

}
