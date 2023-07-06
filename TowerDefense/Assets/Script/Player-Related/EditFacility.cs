using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditFacility : MonoBehaviour
{
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

    [SerializeField] GameObject createObject;

    private GameObject createdObject;
    private bool isObjectMoving; //�I�u�W�F�N�g���ړ������ǂ����̃t���O

    //�������郌�C���[�}�X�N
    private LayerMask ignoreLayers;

    private void Start()
    {
        // Player Layer, Enemy Layer, Defense Layer�𖳎����郌�C���[�}�X�N���쐬
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
