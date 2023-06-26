using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class SensorScript2 : MonoBehaviour
{
    //防衛施設の感知距離、感知角度を表す変数
    //public：施設の強化や敵の能力で変化させるため
    public float searchRadius = 10f;
    public float searchAngle = 70f;

    //防衛施設の初期位置、正面を保存する変数
    private Vector3 defaultPos;
    private Vector3 defaultForward;

    //防衛施設の向きを変更させるボーンオブジェクトを入れる変数
    [SerializeField] private GameObject controlBone;


    //防衛施設の感知範囲(角度は含まない)にあるオブジェクトを入れる配列
    private Collider[] objectsInCollider;

    //感知範囲内にいる敵を入れるリスト
    private List<GameObject> visibleEnemies = new List<GameObject>();

    //visibleEnemiesリスト内で最も距離が近い敵オブジェクトを入れる変数
    private GameObject closestEnemy;


    private void Start()
    {
        //防衛施設の初期位置、正面を保存する
        defaultPos = controlBone.transform.position;
        defaultForward = controlBone.transform.forward;

        //防衛施設の感知範囲半径をSphereCollideコンポーネントから取得する
        searchRadius = gameObject.GetComponent<SphereCollider>().radius;

    }

    private void Update()
    {
        //「transform.position」を中心、「searchRadius」を半径とする球体の内部や触れたすべての「Enemies」マスクを持つコライダーを配列で取得する
        objectsInCollider = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("Enemies"));


        //感知範囲内にいる敵をvisibleEnemiesリストに入れる
        foreach(Collider objectInCollider in objectsInCollider)
        {
            Vector3 directionToEnemy = objectInCollider.transform.position - transform.position;
            directionToEnemy.y = 0;
            float angleToEnemy = Vector3.Angle(defaultForward, directionToEnemy);

            if(angleToEnemy < searchAngle * 0.5f && IsOtherObjectBetween(objectInCollider.gameObject.transform))
            {
                visibleEnemies.Add(objectInCollider.gameObject);
            }
        }

        //visibleEnemiesリスト内の敵の中で最も距離が近い敵をclosestEnemy変数に入れる

    }

    bool IsOtherObjectBetween(Transform target)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target.position, out hit))
        {
            return true;
        }
        return false;
    }


}
