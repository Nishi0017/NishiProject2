using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    [SerializeField] private SensorScript sensorScript;
    [SerializeField] private Transform control;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform appearePos;

    [SerializeField] private float shotInterval;
    private float time = 0f;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int shotDamage;

    
    private GameObject bulletObject;

    private void Update()
    {
        if(sensorScript.canShot)
        {
            time += Time.deltaTime;
            if(time > shotInterval)
            {
                Shot((control.position - transform.position).normalized);
                time = 0f;
            }
        }
    }
    public void Shot(Vector3 shotDistance)
    {
        shotDistance.y = 0;
        bulletObject = Instantiate(bulletPrefab, appearePos.position, Quaternion.identity);
        bulletObject.GetComponent<Rigidbody>().AddForce(shotDistance * shotSpeed, ForceMode.Impulse);
        Destroy(bulletObject, 10.0f);
    }

}
