using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EditFacility : MonoBehaviour
{
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

    [SerializeField] GameObject createObject;

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
    private bool stateEnter = false; //ステート変更してからⅠ回目のフレームであることを表す
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
                
                break;
            
            case EditState.Delete:
                break;

            case EditState.LevelUp:
                break;
        }





    }

    private void UpdatePutState()
    {
        //施設生成待ち
        if (!isObjectMoving)
        {
            //施設の生成
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                RaycastHit hit;

                Vector3 rayOrigin = rightController.transform.position;
                Vector3 rayDirection = rightController.transform.forward;

                if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
                {
                    createdObject = Instantiate(createObject, hit.point, Quaternion.identity);
                    isObjectMoving = true;
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

            //施設の生成キャンセル
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                Destroy(createObject);
                isObjectMoving = false;
            }

            //施設の設置確定
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                isObjectMoving = false;
            }
        }


    }

    private void UpdateDeleteState()
    {

    }

    private void UpdateLevelUpState()
    {

    }
}
