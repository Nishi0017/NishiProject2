using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EditFacility : MonoBehaviour
{
    [SerializeField] private AllFacilityDate allFacilityDate; //施設データ

    //コントローラ
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

    private GameObject createdObject;
    private bool isObjectMoving; //オブジェクトが移動中かどうかのフラグ

    //無視するレイヤーマスク
    private LayerMask ignoreLayers;

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
        ignoreLayers = ~(1 << playerLayer | 1 << enemyLayer | 1 << defenseLayer);
    }

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
                        GameObject createObject = allFacilityDate.facilityDates[selectFacilityNum].facilityPrefab;
                        Debug.Log("クリエイト" + createObject);
                        createdObject = Instantiate(createObject, hit.point, Quaternion.identity);
                        isObjectMoving = true;
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
                createdObject.transform.position = hit.point;
            }

            //施設の設置確定
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
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
