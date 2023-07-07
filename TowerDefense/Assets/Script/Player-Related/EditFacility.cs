using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EditFacility : MonoBehaviour
{
    [SerializeField] private AllFacilityDate allFacilityDate; //�{�݃f�[�^

    //�R���g���[��
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

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
    private bool stateEnter = false; //�X�e�[�g�ύX���Ă���1��ڂ̃t���[���ł��邱�Ƃ�\��
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
        currentState = EditState.Put;
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

    //�X�e�[�g�ڍs��A�P�t���[���ڂ̂ݓ��������߂̏���
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
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {

        }

        //�{�ݐ����҂�
        if (!isObjectMoving)
        {
            //�����{�݂̑I��
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
            {
                selectFacilityNum = (selectFacilityNum + 1) % allFacilityDate.facilityDates.Length;
                Debug.Log("selectFacilityNum�ύX" + selectFacilityNum);
            }

            //�{�݂̐���
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                //������������邩
                int putCost = allFacilityDate.facilityDates[selectFacilityNum].putCost;
                if (GameManager.Instance.HaveMoney >= putCost)
                {
                    RaycastHit hit;

                    Vector3 rayOrigin = rightController.transform.position;
                    Vector3 rayDirection = rightController.transform.forward;

                    if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
                    {
                        GameObject createObject = allFacilityDate.facilityDates[selectFacilityNum].facilityPrefab;
                        Debug.Log("�N���G�C�g" + createObject);
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
            //�{�݂̈ړ�
            RaycastHit hit;

            Vector3 rayOrigin = rightController.transform.position;
            Vector3 rayDirection = rightController.transform.forward;
            
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
            {
                createdObject.transform.position = hit.point;
            }

            //�{�݂̐ݒu�m��
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                int putCost = allFacilityDate.facilityDates[selectFacilityNum].putCost;
                if (GameManager.Instance.UsedMoney(putCost))
                {
                    isObjectMoving = false;
                }
            }

            //�{�݂̐����L�����Z��
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                Destroy(createdObject);
                isObjectMoving = false;
            }
        }


    }

    private void UpdateDeleteState()
    {
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {

        }
    }

    private void UpdateLevelUpState()
    {
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {

        }
    }


}
