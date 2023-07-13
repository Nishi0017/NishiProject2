using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EditFacility2 : MonoBehaviour
{
    [SerializeField] private AllFacilityDate allFacilityDate; //施設データ

    //コントローラ
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

    //カメラ
    [SerializeField] private Transform centerCamera;

    //設置確定orキャンセルUIのプレハブ
    [SerializeField] private GameObject confirmPlacementUIPrefab;
    private GameObject confirmPlacementUI; //設置の確定、キャンセルのUIを保存する変数

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
        currentState = EditState.None;
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
                ChangeState(EditState.Put);
                break;

            case EditState.Put:
                UpdatePut();
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


    

    #region PutState処理
    private bool canCreate = false;　//オブジェクトが作成可能かどうかのフラグ
    private bool canObjectMove = false; //オブジェクトが移動中かどうかのフラグ
    //private Vector3 oldLayerNormal = new Vector3(0, 0, 0); //Rayが当たったレイヤーの法線を保存し、変更があったかどうかを管理するための変数
    private int selectFacilityNum = 0; //設置する施設のID
    private GameObject createdObject; //作成した施設を保存する変数
    private void UpdatePut()
    {
        //ステート移行の際に一回だけ実行
        if (stateEnter)
        {
            canCreate = true;
            canObjectMove = false;
        }

        //生成施設の変更処理
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

            if (createdObject != null)
            {
                Destroy(createdObject); createdObject = null;
                canCreate= true;
                canObjectMove = false;
                CreateFacility();
            }

        }

        if (canCreate)
        {
            //施設生成
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                CreateFacility();
            }
        }
        else
        {
            //施設の移動処理
            if (canObjectMove)
            {
                RaycastHit hit;

                Vector3 rayOrigin = rightController.transform.position;
                Vector3 rayDirection = rightController.transform.forward;

                if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
                {
                    //置ける場所以外に施設がいないようにする処理
                    int hitLayer = hit.collider.gameObject.layer;
                    if (allowedPlacemenLayer == (allowedPlacemenLayer | (1 << hitLayer)))
                    {
                        createdObject.transform.position = hit.point;
                        if(hit.normal != createdObject.transform.up)
                        {
                            createdObject.transform.up = hit.normal;
                        }
                    }
                }

                //設置確定orキャンセルのUIの表示
                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    Vector3 uiPos = centerCamera.position + (createdObject.transform.position - centerCamera.position).normalized * 2;
                    Vector3 toPlayerDistance = createdObject.transform.position - centerCamera.position;
                    confirmPlacementUI = Instantiate(confirmPlacementUIPrefab, uiPos, Quaternion.LookRotation(toPlayerDistance));

                    canObjectMove = false;
                }
            }


        }
    }

    /// <summary>
    /// 施設の生成
    /// </summary>
    private void CreateFacility()
    {
        RaycastHit hit;

        Vector3 rayOrigin = rightController.transform.position;
        Vector3 rayDirection = rightController.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
        {
            int hitLayer = hit.collider.gameObject.layer;

            //当たったレイヤーの位置に施設が置けるかどうかの条件分岐
            if (allowedPlacemenLayer == (allowedPlacemenLayer | (1 << hitLayer)))
            {
                GameObject createObject = allFacilityDate.facilityDates[selectFacilityNum].facilityPrefab;
                createdObject = Instantiate(createObject, hit.point, Quaternion.identity);　//施設の設置
                createdObject.transform.up = hit.normal;
                canCreate = false;
                canObjectMove = true;
            }
        }
        else //置ける場所ではないときの処理
        {
            Debug.Log("おけない場所だよ");
        }
    }

    public void PutConfirm()
    {
        createdObject = null;
        Destroy(confirmPlacementUI); confirmPlacementUI = null;
        canCreate = true;
        canObjectMove = false;
    }

    public void PutCancel()
    {
        Destroy(createdObject); createdObject = null;
        Destroy(confirmPlacementUI); confirmPlacementUI = null;
        canCreate = true;
        canObjectMove = false;
    }
    #endregion


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
