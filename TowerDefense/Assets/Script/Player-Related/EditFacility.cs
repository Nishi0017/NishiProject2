using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EditFacility : MonoBehaviour
{
    [SerializeField] private AllFacilityDate allFacilityDate; //施設データ

    //コントローラ
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

    private GameObject createdObject; //作成した施設を保存する変数
    private bool isObjectMoving; //オブジェクトが移動中かどうかのフラグ

    //無視するレイヤーマスク
    private LayerMask ignoreLayers;

    //防衛施設をおける場所のレイヤーマスク(置く防衛施設によって変化)
    private LayerMask allowedPlacemenLayer;


    //Stateの種類
    public enum EditState
    {
        None,
        Put,
        Delete,
        LevelUp
    }
    private EditState currentState; //現在のステート
    private bool stateEnter = false; //ステート変更してから1回目のフレームであることを表す
    private float stateTime = 0.0f; //ステートに移行してからの時間を保存

    /// <summary>
    /// ステート移行関数
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(EditState newState)
    {
        currentState = newState;
        stateEnter = true;
        stateTime = 0.0f;
        Debug.Log("現在のステート" + currentState);
    }

    private void Start()
    {
        currentState = EditState.Put;
        Debug.Log("現在のステート" + currentState);

        // Player Layer, Enemy Layer, Defense Layerを無視するレイヤーマスクを作成
        int playerLayer = LayerMask.NameToLayer("PlayerLayer");
        int enemyLayer = LayerMask.NameToLayer("EnemyLayer");
        int defenseLayer = LayerMask.NameToLayer("DefenseLayer");
        ignoreLayers = ~(1 << playerLayer) | (1 << enemyLayer) | (1 << defenseLayer);

        //置けるエリアの初期化
        var facilityDate = allFacilityDate.facilityDates[0];

        int wallLayer = LayerMask.NameToLayer("WallLayer");
        allowedPlacemenLayer = facilityDate.canPutWall ? (allowedPlacemenLayer | (1 << wallLayer)) : (allowedPlacemenLayer & ~(1 << wallLayer));

        int floorLayer = LayerMask.NameToLayer("FloorLayer");
        allowedPlacemenLayer = facilityDate.canPutFloor ? (allowedPlacemenLayer | (1 << floorLayer)) : (allowedPlacemenLayer & ~(1 << floorLayer));
        
        int roofLayer = LayerMask.NameToLayer("RoofLayer");
        allowedPlacemenLayer = facilityDate.canPutRoof ? (allowedPlacemenLayer | (1 << roofLayer)) : (allowedPlacemenLayer & ~(1 << roofLayer));
        
    }
    /*
    メモ
    (レイヤーマスク) |= (1 << レイヤー);でレイヤーマスクにレイヤーを追加する
    (レイヤーマスク) &= ~(1 << レイヤー);でレイヤーマスクからレイヤーを削除する
    (レイヤーマスク) == ((レイヤーマスク) | (1 << (レイヤー)))でレイヤーマスクにレイヤーが含まれているかどうか
    */

    private void Update()
    {
        stateTime = Time.deltaTime;

        switch (currentState)
        {
            case EditState.None:
                
                break;

            case EditState.Put:
                UpdatePutState();
                break;
            
            case EditState.Delete:
                UpdateDeleteState();
                break;

            case EditState.LevelUp:
                UpdateLevelUpState();
                break;
        }





    }

    //ステート移行後、１フレーム目のみ動かすための処理
    private void LateUpdate()
    {
        if (stateTime != 0.0f && stateEnter)
        {
            stateEnter = false;
        }
    }


    private int selectFacilityNum = 0;
    private void UpdatePutState()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {

        }

        //施設生成待ち
        if (!isObjectMoving)
        {
            //生成施設の選択
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
            {
                selectFacilityNum = (selectFacilityNum + 1) % allFacilityDate.facilityDates.Length;
                Debug.Log("selectFacilityNum変更" + selectFacilityNum);

                //置けるエリアの更新
                var facilityDate = allFacilityDate.facilityDates[selectFacilityNum];
                int wallLayer = LayerMask.NameToLayer("WallLayer");
                allowedPlacemenLayer = facilityDate.canPutWall ? (allowedPlacemenLayer | (1 << wallLayer)) : (allowedPlacemenLayer & ~(1 << wallLayer));

                int floorLayer = LayerMask.NameToLayer("FloorLayer");
                allowedPlacemenLayer = facilityDate.canPutFloor ? (allowedPlacemenLayer | (1 << floorLayer)) : (allowedPlacemenLayer & ~(1 << floorLayer));

                int roofLayer = LayerMask.NameToLayer("RoofLayer");
                allowedPlacemenLayer = facilityDate.canPutRoof ? (allowedPlacemenLayer | (1 << roofLayer)) : (allowedPlacemenLayer & ~(1 << roofLayer));

                for (int i = 0; i < 32; i++)
                {
                    if (allowedPlacemenLayer == (allowedPlacemenLayer | (1 << i)))
                    {
                        Debug.Log("含まれている:" + i);
                    }
                }
            }

            //施設の生成
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                //所持金が足りるか
                int putCost = allFacilityDate.facilityDates[selectFacilityNum].putCost;
                if (GameManager.Instance.HaveMoney >= putCost)
                {
                    RaycastHit hit;

                    Vector3 rayOrigin = rightController.transform.position;
                    Vector3 rayDirection = rightController.transform.forward;

                    if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
                    {
                        int hitLayer = hit.collider.gameObject.layer;
                        Debug.Log("当たったオブジェクト" + hit.collider.gameObject.name);
                        Debug.Log("hitLayer" + hitLayer);
                        if (allowedPlacemenLayer == (allowedPlacemenLayer | (1 << hitLayer)))
                        {
                            GameObject createObject = allFacilityDate.facilityDates[selectFacilityNum].facilityPrefab;
                            Debug.Log("クリエイト" + createObject);
                            createdObject = Instantiate(createObject, hit.point, Quaternion.identity);
                            isObjectMoving = true;
                        }
                    }
                }
                else
                {

                }

            }
        }
        else
        {
            //施設の移動
            RaycastHit hit;

            Vector3 rayOrigin = rightController.transform.position;
            Vector3 rayDirection = rightController.transform.forward;
            
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
            {
                int hitLayer = hit.collider.gameObject.layer;
                if (allowedPlacemenLayer == (allowedPlacemenLayer | (1 << hitLayer)))
                {
                    createdObject.transform.position = hit.point;
                }

                
            }

            //施設の設置確定
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                //所持金を減らし、施設が動かないようにする処理
                int putCost = allFacilityDate.facilityDates[selectFacilityNum].putCost;
                if (GameManager.Instance.UsedMoney(putCost))
                {
                    isObjectMoving = false;
                }
            }

            //施設の生成キャンセル
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                Destroy(createdObject);
                isObjectMoving = false;
            }
        }


    }

    private void UpdateDeleteState()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {

        }
    }

    private void UpdateLevelUpState()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {

        }
    }


}
