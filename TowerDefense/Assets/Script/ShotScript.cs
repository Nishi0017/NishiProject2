using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    //�T�E���h�֌W
    private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;
    
    //�A�j���[�V�����֌W
    private Animator animator;
    [SerializeField] private float AnimSpeed = 1.0f;

    //���ˊ֌W
    [SerializeField] private SensorScript sensorScript;
    [SerializeField] private bool canShot = false;
    [SerializeField] private GameObject bulletPrefab; //�e�̃v���n�u
    [SerializeField] private Transform ctrlBone; //���˕����v�Z�p
    [SerializeField] private Transform bulletSpawnPoint; //�e���o�Ă���ʒu
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shotInterval;
    [SerializeField] private int bulletDamage;

    //�ʒu�ύX�𔻒f���邽�߂̊֐�
    Vector3 lastPos;


    private float timer = 0.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = AnimSpeed;
            animator.SetBool("CanShot", false);
        }
    
        lastPos = transform.position;
    }

    private void Update()
    {
        //�A�^�b�`�s���ɂ��G���[���
        if (!CheckInitialConditions())
        {
            return;
        }

        if(lastPos != transform.position)
        {
            sensorScript.ResetCtrlPos();
            lastPos = transform.position;
        }
        
        //clsestEnemy�����݂��邩�AcanShot�̒l���ύX�O��ňقȂ�ꍇ�ύX
        if(canShot != (sensorScript.closestEnemy != null))
        {
            canShot = (sensorScript.closestEnemy != null);

            //�A�j���[�V�����̍Đ��A��~��canShot�ɍ��킹��
            if(animator != null)
            {
                if (animator.GetBool("CanShot") != canShot)
                {
                    animator.SetBool("CanShot", canShot);
                }
            }

            //timer�̃��Z�b�g
            if(canShot)
            {
                timer = 0.0f;
            }

            return;
        }

        if(!canShot) 
        {
            return;
        }

        //�e��ł���
        timer += Time.deltaTime;
        if (timer > shotInterval)
        {
            Shot((ctrlBone.position - bulletSpawnPoint.transform.position).normalized);
            timer = 0.0f;
        }

    }

    /// <summary>
    /// �e��shotDistance�̕����ɑł�
    /// </summary>
    /// <param name="shotDistance"></param>
    private void Shot(Vector3 shotDistance)
    {
        audioSource.PlayOneShot(shotSound);
        shotDistance.y = 0.0f;
        GameObject bulletObject = Instantiate
            (
                bulletPrefab, 
                bulletSpawnPoint.position, 
                Quaternion.LookRotation((ctrlBone.position - bulletSpawnPoint.position).normalized)
            );
        bulletObject.GetComponent<Rigidbody>().AddForce(shotDistance * bulletSpeed, ForceMode.Impulse);
        bulletObject.GetComponent<BulletScript>().inputDamageAmount(bulletDamage);
        Destroy (bulletObject, 10.0f);

    }

    /// <summary>
    /// ���������������Ă��邩�̊m�F����
    /// </summary>
    /// <returns></returns>
    bool CheckInitialConditions()
    {
        if(sensorScript == null || bulletPrefab == null || bulletSpawnPoint == null)
        {
            return false;
        }
        return true;
    }

}
