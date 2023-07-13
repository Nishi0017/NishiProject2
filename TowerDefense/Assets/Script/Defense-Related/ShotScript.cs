using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    //サウンド関係
    private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound; //発射時の音
    
    //アニメーション関係
    private Animator animator;
    [SerializeField] private float AnimSpeed = 1.0f; //アニメーション速度

    //発射関係
    [SerializeField] private SensorScript sensorScript;
    [SerializeField] private bool canShot = false;
    [SerializeField] private GameObject bulletPrefab; //弾のプレハブ
    [SerializeField] private Transform ctrlBone; //発射方向計算用
    [SerializeField] private Transform bulletSpawnPoint; //弾が出てくる位置
    [SerializeField] private float bulletSpeed; //弾速度
    [SerializeField] private float shotInterval; //発射感覚
    [SerializeField] private int bulletDamage; //ダメージ量

    //位置変更を判断するための関数
    Vector3 lastPos;


    private float timer = 0.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //アニメーションがあるかの条件分岐
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
        //アタッチ不足によるエラー回避
        if (!CheckInitialConditions())
        {
            return;
        }

        //ctrlBoneの初期行き更新
        if(lastPos != transform.position)
        {
            sensorScript.ResetCtrlPos();
            lastPos = transform.position;
        }
        
        //clsestEnemyが存在するかつ、canShotの値が変更前後で異なる場合変更
        if(canShot != (sensorScript.closestEnemy != null))
        {
            canShot = (sensorScript.closestEnemy != null);

            //アニメーションの再生、停止をcanShotに合わせる
            if(animator != null)
            {
                if (animator.GetBool("CanShot") != canShot)
                {
                    animator.SetBool("CanShot", canShot);
                }
            }

            //timerのリセット
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

        //弾を打つ処理
        timer += Time.deltaTime;
        if (timer > shotInterval)
        {
            Shot((ctrlBone.position - bulletSpawnPoint.transform.position).normalized);
            timer = 0.0f;
        }

    }

    /// <summary>
    /// 弾をshotDistanceの方向に打つ
    /// </summary>
    /// <param name="shotDistance"></param>
    private void Shot(Vector3 shotDistance)
    {
        audioSource.PlayOneShot(shotSound);
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
    /// 初期条件があっているかの確認処理
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
