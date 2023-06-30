using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage = 0;

    public void inputDamageAmount(int _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
