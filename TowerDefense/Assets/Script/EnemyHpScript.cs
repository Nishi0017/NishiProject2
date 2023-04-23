using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpScript : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    private int currentHp;
    [SerializeField] private Slider slider;

    [SerializeField] private int enemyDef = 0;


    private void Start()
    {
        slider.value = 1;
        currentHp = maxHp;
    }

    public void Damage(int _damage)
    {
        currentHp -= _damage;
        if(currentHp < 0)
        {
            Destroy(gameObject);
        }
        slider.value = (float)currentHp / (float)maxHp;
    }
}
