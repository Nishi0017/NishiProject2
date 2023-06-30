using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SensorScript : MonoBehaviour
{
    //rayShotかどうか
    [SerializeField] bool rayShot = false;

    //防衛施設の感知距離、感知角度を表す変数
    //public：施設の強化や敵の能力で変化させるため
    public float searchRadius = 10f;
    public float searchAngle = 70f;

    //防衛施設が敵の方向向く速度(0.1f～1.0fで値が大きいほど早い)
    [Range(0.1f, 1.0f)] public float aimingSpeed = 0.5f;
     
    //防衛施設の初期位置、正面を保存する変数
    private Vector3 defaultPos;
    private Vector3 defaultForward;

    //防衛施設の向きを変更させるボーンオブジェクトを入れる変数
    [SerializeField] private GameObject ctrlBone;

    //感知範囲内にいる敵を入れるリスト
    private List<GameObject> visibleEnemies = new List<GameObject>();

    //visibleEnemiesリスト内で最も距離が近い敵オブジェクトを入れる変数
    public GameObject closestEnemy;

    private void Start()
    {
        //防衛施設の初期位置、正面を保存する
        defaultPos = ctrlBone.transform.position;
        defaultForward = ctrlBone.transform.forward;

        //防衛施設の感知範囲半径をSphereCollideコンポーネントから取得する
        searchRadius = gameObject.GetComponent<SphereCollider>().radius;

    }

    private void Update()
    {
        //「transform.position」を中心、「searchRadius」を半径とする球体の内部や
        //触れたすべての「Enemies」マスクを持つコライダーを
        //防衛施設の感知範囲(角度は含まない)にあるオブジェクトを入れる配列にいれる
        Collider[] objectsInCollider = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("EnemyLayer"));


        
        //感知範囲内にいる敵をvisibleEnemiesリストに入れる
        foreach (Collider objectInCollider in objectsInCollider)
        {
            Vector3 directionToEnemy = objectInCollider.transform.position - transform.position;
            directionToEnemy.y = 0;
            float angleToEnemy = Vector3.Angle(defaultForward, directionToEnemy);

            if ((searchAngle * -0.5f <= angleToEnemy || angleToEnemy <= searchAngle * 0.5f) && !IsOtherObjectBetween(objectInCollider.gameObject.transform))
            {
                //既にListに入っている敵を新たにListに入れないための条件分岐
                if (!visibleEnemies.Contains(objectInCollider.gameObject))
                {
                    visibleEnemies.Add(objectInCollider.gameObject);
                }
            }
        }
        visibleEnemies.RemoveAll(enemy => enemy == null);

        if (visibleEnemies.Count != 0)
        {
            //visibleEnemiesリスト内の敵の中で最も距離が近い敵をclosestEnemy変数に入れる
            closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in visibleEnemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToEnemy;
                }
            }


            //防衛施設の向きを最も近い敵に向ける
            if (closestEnemy != null)
            {
                //オブジェクトの衝突によるダメージか、Rayの衝突によるダメージかでの場合分け
                Vector3 closestEnemyPos = rayShot ? closestEnemy.transform.position : closestEnemy.transform.GetChild(0).transform.position;

                closestEnemyPos = new Vector3(closestEnemyPos.x, defaultPos.y, closestEnemyPos.z);
                ctrlBone.transform.position = Vector3.Lerp(ctrlBone.transform.position, closestEnemyPos, aimingSpeed);
            }
        }
        else if(ctrlBone.transform.position != defaultPos)
        {
            ctrlBone.transform.position = Vector3.Lerp(ctrlBone.transform.position, defaultPos, 0.1f);
            if(closestEnemy != null)
            {
                closestEnemy = null;
            }
        
        }

        //防衛施設の感知範囲外に出た敵をVisibleEnemiesから削除し、感知範囲内に残っている敵のみを残す
        List<GameObject> remainingEnemies = new List<GameObject>();
        foreach (GameObject enemy in visibleEnemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            directionToEnemy.y = 0;
            float angleToEnemy = Vector3.Angle(defaultForward, directionToEnemy);

            float distanceToEnemy = directionToEnemy.sqrMagnitude;

            if ((searchAngle * -0.5f <= angleToEnemy || angleToEnemy <= searchAngle * 0.5f) && distanceToEnemy <= searchRadius * searchRadius && !IsOtherObjectBetween(enemy.transform))
            {
                remainingEnemies.Add(enemy.gameObject);
            }
        }
        visibleEnemies = remainingEnemies;

    }


    /// <summary>
    /// 敵との間にWallタグを持ったオブジェクトがあったらtrue,なかったらfalseを返す
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    bool IsOtherObjectBetween(Transform target)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, target.position, out hit, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 防衛施設の初期位置、正面を保存する
    /// </summary>
    public void ResetCtrlPos()
    {
        defaultPos = ctrlBone.transform.position;
        defaultForward = ctrlBone.transform.forward;
    }

}
