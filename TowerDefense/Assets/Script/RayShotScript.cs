using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShotScript : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;

    

    [SerializeField] private SensorScript sensorScript;
    [SerializeField] private Transform gunpoint;
    private GameObject target = null;
    private EnemyHpScript hpScript;

    [SerializeField] private float damageInterval;
    private float time = 0f;
    [SerializeField] private int shotDamage;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        /*
        if (!sensorScript.canShot)
        {
            return;
        }
        */

        Ray ray = new Ray(gunpoint.position, gunpoint.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(shotSound);
                }        
                if (hit.collider.gameObject != target)
                {
                    target = hit.collider.gameObject;
                    hpScript = target.GetComponent<EnemyHpScript>();
                }
                time += Time.deltaTime;
                if(time > damageInterval)
                {

                    hpScript.Damage(shotDamage);
                    time = 0f;
                }
            }
            else if(target != null)
            {
                target = null;
            }
        }
    }
}
