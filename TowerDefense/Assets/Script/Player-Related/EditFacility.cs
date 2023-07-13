using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EditFacility : MonoBehaviour
{
    [SerializeField] private AllFacilityDate allFacilityDate; //�{�݃f�[�^

    //�R���g���[��
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

    private GameObject createdObject; //�쐬�����{�݂�ۑ�����ϐ�
    private bool isObjectMoving; //�I�u�W�F�N�g���ړ������ǂ����̃t���O

    //�������郌�C���[�}�X�N
    private LayerMask ignoreLayers;

    //�h�q�{�݂�������ꏊ�̃��C���[�}�X�N(�u���h�q�{�݂ɂ���ĕω�)
    private LayerMask allowedPlacemenLayer;


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
        ignoreLayers = ~(1 << playerLayer) | (1 << enemyLayer) | (1 << defenseLayer);

        //�u����G���A�̏�����
        var facilityDate = allFacilityDate.facilityDates[0];

        int wallLayer = LayerMask.NameToLayer("WallLayer");
        allowedPlacemenLayer = facilityDate.canPutWall ? (allowedPlacemenLayer | (1 << wallLayer)) : (allowedPlacemenLayer & ~(1 << wallLayer));

        int floorLayer = LayerMask.NameToLayer("FloorLayer");
        allowedPlacemenLayer = facilityDate.canPutFloor ? (allowedPlacemenLayer | (1 << floorLayer)) : (allowedPlacemenLayer & ~(1 << floorLayer));
        
        int roofLayer = LayerMask.NameToLayer("RoofLayer");
        allowedPlacemenLayer = facilityDate.canPutRoof ? (allowedPlacemenLayer | (1 << roofLayer)) : (allowedPlacemenLayer & ~(1 << roofLayer));
        
    }
    /*
    ����
    (���C���[�}�X�N) |= (1 << ���C���[);�Ń��C���[�}�X�N�Ƀ��C���[��ǉ�����
    (���C���[�}�X�N) &= ~(1 << ���C���[);�Ń��C���[�}�X�N���烌�C���[���폜����
    (���C���[�}�X�N) == ((���C���[�}�X�N) | (1 << (���C���[)))�Ń��C���[�}�X�N�Ƀ��C���[���܂܂�Ă��邩�ǂ���
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

                //�u����G���A�̍X�V
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
                        Debug.Log("�܂܂�Ă���:" + i);
                    }
                }
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
                        int hitLayer = hit.collider.gameObject.layer;
                        Debug.Log("���������I�u�W�F�N�g" + hit.collider.gameObject.name);
                        Debug.Log("hitLayer" + hitLayer);
                        if (allowedPlacemenLayer == (allowedPlacemenLayer | (1 << hitLayer)))
                        {
                            GameObject createObject = allFacilityDate.facilityDates[selectFacilityNum].facilityPrefab;
                            Debug.Log("�N���G�C�g" + createObject);
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
            //�{�݂̈ړ�
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

            //�{�݂̐ݒu�m��
            if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
                //�����������炵�A�{�݂������Ȃ��悤�ɂ��鏈��
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
