using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        // Player Layer, Enemy Layer, Defense Layerを無視するレイヤーマスクを作成
        int playerLayer = LayerMask.NameToLayer("PlayerLayer");
        int enemyLayer = LayerMask.NameToLayer("EnemyLayer");
        int defenseLayer = LayerMask.NameToLayer("DefenseLayer");
        ignoreLayers = ~(1 << playerLayer | 1 << enemyLayer | 1 << defenseLayer);
    }

    private void Update()
    {
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

        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            RaycastHit hit;

            Vector3 rayOrigin = rightController.transform.position;
            Vector3 rayDirection = rightController.transform.forward;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
            {
                createdObject.transform.position = hit.point;
            }
        }

        if(OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            isObjectMoving= false;
        }

        if(isObjectMoving)
        {

        }

    }
}
