using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShotScript2 : MonoBehaviour
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
    [SerializeField] private Transform raySpawnPoint;
    [SerializeField] private float damageInterval;
    [SerializeField] private int rayDamage;

    private GameObject targetEnemy = null;
    EnemyHpScript enemyHpScript;

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
        if (sensorScript.closestEnemy == null)
        {
            return;
        }

        //clsestEnemy�����݂��邩�AcanShot�̒l���ύX�O��ňقȂ�ꍇ�ύX
        if (canShot != (sensorScript.closestEnemy != null))
        {
            canShot = (sensorScript.closestEnemy != null);

            //�A�j���[�V�����̍Đ��A��~��canShot�ɍ��킹��
            if (animator != null)
            {
                if (animator.GetBool("CanShot") != canShot)
                {
                    animator.SetBool("CanShot", canShot);
                }
            }

            //timer�̃��Z�b�g
            if (canShot)
            {
                timer = 0.0f;
            }
        }

        if (!canShot)
        {
            return;
        }

        timer += Time.deltaTime;
        //Ray�ɂ��_���[�W����
        RaycastHit hit;
        if(Physics.Raycast(raySpawnPoint.position, raySpawnPoint.forward, out hit))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                //shot����G��Ray���������Ă���Ƃ��Ɏ����I�ɂȂ炷
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(shotSound);
                }
                
                //�_���[�W��^����G�̍X�V����
                if (hit.collider.gameObject != targetEnemy)
                {
                    targetEnemy = hit.collider.gameObject;
                    enemyHpScript = hit.collider.gameObject.GetComponent<EnemyHpScript>();
                }

                //�����I�Ƀ_���[�W��^����
                if (timer > damageInterval)
                {
                    enemyHpScript.Damage(rayDamage);
                    timer = 0.0f;
                }
            }
            else if(targetEnemy != null)
            {
                targetEnemy = null;
            }
        }

    }

    /// <summary>
    /// ���������������Ă��邩�̊m�F����
    /// </summary>
    /// <returns></returns>
    bool CheckInitialConditions()
    {
        if(sensorScript == null || raySpawnPoint == null)
        {
            return false;
        }
        return true;
    }

}
