﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayShotScript : MonoBehaviour
{
    //サウンド関係
    private AudioSource audioSource;
    [SerializeField] private AudioClip shotSound; //発射時の音

    //アニメーション関係
    private Animator animator;
    [SerializeField] private float AnimSpeed = 1.0f;　//アニメーション速度

    //発射関係
    [SerializeField] private SensorScript sensorScript;
    public bool canShot = false;
    [SerializeField] private Transform raySpawnPoint; //Rayを発射し始める位置
    private float maxRange;　//射程範囲
    [SerializeField] private float damageInterval;
    [SerializeField] private int rayDamage;
    //Rayが当たらないMask設定用
    private int defenseLayer;
    private int enemyLayer;
    private int layerMask;

    //Rayが当たった敵の保存用
    private GameObject targetEnemy = null;
    EnemyHpScript enemyHpScript;

    private float timer = 0.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        //アニメーションがあるかどうかの条件分岐
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = AnimSpeed;
            animator.SetBool("CanShot", false);
        }

        maxRange = sensorScript.searchRadius;

        //Rayが当たらないようにするための初期設定
        defenseLayer = LayerMask.NameToLayer("DefenseLayer");
        enemyLayer = LayerMask.NameToLayer("EnemyLayer");
        layerMask = ~(1 << defenseLayer);
    }

    private void Update()
    {
        //アタッチ不足によるエラー回避
        if (!CheckInitialConditions())
        {
            return;
        }

        //防衛施設の感知範囲内に敵がいない場合何もしない処理
        if (sensorScript.closestEnemy == null)
        {
            return;
        }

        //clsestEnemyが存在するかつ、canShotの値が変更前後で異なる場合変更
        if (canShot != (sensorScript.closestEnemy != null))
        {
            canShot = (sensorScript.closestEnemy != null);

            //アニメーションの再生、停止をcanShotに合わせる
            if (animator != null)
            {
                if (animator.GetBool("CanShot") != canShot)
                {
                    animator.SetBool("CanShot", canShot);
                }
            }

            //timerのリセット
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
        //Rayによるダメージ処理
        RaycastHit hit;
        if(Physics.Raycast(raySpawnPoint.position, raySpawnPoint.up, out hit, maxRange, layerMask))
        {
            if (hit.collider.gameObject.layer == enemyLayer)
            {
                //shot音を敵にRayが当たっているときに周期的にならす
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(shotSound);
                }
                
                //ダメージを与える敵の更新処理
                if (hit.collider.gameObject != targetEnemy)
                {
                    targetEnemy = hit.collider.gameObject;
                    enemyHpScript = hit.collider.gameObject.GetComponent<EnemyHpScript>();
                }

                //周期的にダメージを与える
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
    /// 初期条件があっているかの確認処理
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
