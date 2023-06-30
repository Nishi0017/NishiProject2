using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayShotScript : MonoBehaviour
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
    private float maxRange;
    [SerializeField] private float damageInterval;
    [SerializeField] private int rayDamage;

    private int defenseLayer;
    private int enemyLayer;
    private int layerMask;

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
        maxRange = sensorScript.searchRadius;
        defenseLayer = LayerMask.NameToLayer("DefenseLayer");
        enemyLayer = LayerMask.NameToLayer("EnemyLayer");
        layerMask = ~(1 << defenseLayer);
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
        if(Physics.Raycast(raySpawnPoint.position, raySpawnPoint.up, out hit, maxRange, layerMask))
        {
            if (hit.collider.gameObject.layer == enemyLayer)
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
                if (timer > damageInterval && enemyHpScript != null)
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
