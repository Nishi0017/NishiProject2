using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class ShotScript1 : MonoBehaviour
{
    //サウンド関係
    private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound;
    
    //アニメーション関係
    private Animator animator;
    [SerializeField] private float AnimSpeed = 1.0f;

    //発射関係
    [SerializeField] private SensorScript sensorScript;
    public bool canShot = false;
    [SerializeField] private GameObject bulletPrefab; //弾のプレハブ
    [SerializeField] private Transform ctrlBone; //発射方向計算用
    [SerializeField] private Transform bulletSpawnPoint; //弾が出てくる位置
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
        //アタッチ不足によるエラー回避
        if (!CheckInitialConditions())
        {
            return;
        }

        //防衛施設の感知範囲内に敵がいない場合何もしない処理
        if(sensorScript.closestEnemy == null)
        {
            return;
        }

        //アニメーションを持つ防衛施設とそうでない防衛施設で条件分岐
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
