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
    private bool isObjectMoving; //�I�u�W�F�N�g���ړ������ǂ����̃t���O

    //�������郌�C���[�}�X�N
    private LayerMask ignoreLayers;

    //State�̎��
    public enum EditState
    {
        None,
        Put,
        Delete,
        LevelUp
    }
    private EditState currentState; //���݂̃X�e�[�g
    private bool stateEnter = false; //�X�e�[�g�ύX���Ă���T��ڂ̃t���[���ł��邱�Ƃ�\��
    private float stateTime = 0.0f; //�X�e�[�g�Ɉڍs���Ă���̎��Ԃ�ۑ�

    /// <summary>
    /// �X�e�[�g�ڍs�֐�
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(EditState newState)
    {
        currentState = newState;
        stateEnter = true;
        stateTime = 0.0f;
        Debug.Log("���݂̃X�e�[�g" + currentState);
    }

    private void Start()
    {
        currentState = EditState.None;
        Debug.Log("���݂̃X�e�[�g" + currentState);

        // Player Layer, Enemy Layer, Defense Layer�𖳎����郌�C���[�}�X�N���쐬
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
        //�{�ݐ����҂�
        if (!isObjectMoving)
        {
            //�{�݂̐���
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
            //�{�݂̈ړ�
            RaycastHit hit;

            Vector3 rayOrigin = rightController.transform.position;
            Vector3 rayDirection = rightController.transform.forward;
            
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
            {
                createdObject.transform.position = hit.point;
            }

            //�{�݂̐����L�����Z��
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                Destroy(createObject);
                isObjectMoving = false;
            }

            //�{�݂̐ݒu�m��
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
