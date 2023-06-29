using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ShotScript2 : MonoBehaviour
{
    /*
    private AudioSource audioSource;
    [SerializeField] private bool haveAnimation;
    private Animator animator;

    [SerializeField] private AudioClip shotSound;

    [SerializeField] private SensorScript sensorScript;
    [SerializeField] private Transform control;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform appearePos;

    [SerializeField] private float shotInterval;
    private float time = 0f;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int shotDamage;


    private GameObject bulletObject;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (haveAnimation)
        {
            animator = GetComponent<Animator>();
            animator.SetBool("CanShot", false);
        }
    }

    private void Update()
    {
        if (sensorScript.canShot)
        {
            if (haveAnimation && !animator.GetBool("CanShot"))
            {
                animator.SetBool("CanShot", true);
            }
            time += Time.deltaTime;
            if (time > shotInterval)
            {
                Shot((control.position - appearePos.transform.position).normalized);
                time = 0f;
            }
        }
        else if (haveAnimation && animator.GetBool("CanShot"))
        {
            animator.SetBool("CanShot", false);
        }
    }
    public void Shot(Vector3 shotDistance)
    {

        audioSource.PlayOneShot(shotSound);
        shotDistance.y = 0;
        bulletObject = Instantiate(bulletPrefab, appearePos.position, Quaternion.LookRotation((control.position - appearePos.transform.position).normalized));
        bulletObject.GetComponent<Rigidbody>().AddForce(shotDistance * shotSpeed, ForceMode.Impulse);
        Destroy(bulletObject, 10.0f);
    }
    */
}
