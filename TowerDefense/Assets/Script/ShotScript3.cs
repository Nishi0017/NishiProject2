using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ShotScript1 : MonoBehaviour
{
    //�T�E���h�֌W
    private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;
    
    //�A�j���[�V�����֌W
    private Animator animator;
    [SerializeField] private float AnimSpeed = 1.0f;

    //���ˊ֌W
    [SerializeField] private SensorScript sensorScript;
    public bool canShot = false;
    [SerializeField] private GameObject bulletPrefab; //�e�̃v���n�u
    [SerializeField] private Transform ctrlBone; //���˕����v�Z�p
    [SerializeField] private Transform bulletSpawnPoint; //�e���o�Ă���ʒu
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shotInterval;
    [SerializeField] private float bulletDamage;


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
    }

    private void Update()
    {
        //�A�^�b�`�s���ɂ��G���[���
        if (!CheckInitialConditions())
        {
            return;
        }

        //�h�q�{�݂̊��m�͈͓��ɓG�����Ȃ��ꍇ�������Ȃ�����
        if(sensorScript.closestEnemy == null)
        {
            return;
        }

        //�A�j���[�V���������h�q�{�݂Ƃ����łȂ��h�q�{�݂ŏ�������
        if(animator != null)
        {

        }
        else
        {
            Update_haveAnim();
        }
    }

    private void Update_haveAnim()
    {

    }

    private void Shot(Vector3 shotDistance)
    {

    }

    void ChangeAnimState()
    {
        animator.SetBool("CanShot", canShot);
    }

    bool CheckInitialConditions()
    {
        if(sensorScript == null || bulletPrefab == null || bulletSpawnPoint == null)
        {
            return false;
        }
        return true;
    }

}
