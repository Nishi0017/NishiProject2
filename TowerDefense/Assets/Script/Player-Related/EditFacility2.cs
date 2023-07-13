using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EditFacility2 : MonoBehaviour
{
    [SerializeField] private AllFacilityDate allFacilityDate; //�{�݃f�[�^

    //�R���g���[��
    [SerializeField] private GameObject rightController;
    [SerializeField] private GameObject leftController;

    //�J����
    [SerializeField] private Transform centerCamera;

    //�ݒu�m��or�L�����Z��UI�̃v���n�u
    [SerializeField] private GameObject confirmPlacementUIPrefab;
    private GameObject confirmPlacementUI; //�ݒu�̊m��A�L�����Z����UI��ۑ�����ϐ�

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
        currentState = EditState.None;
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

    //�X�e�[�g�ڍs��A�P�t���[���ڂ̂ݓ��������߂̏���
    private void LateUpdate()
    {
        if (stateTime != 0.0f && stateEnter)
        {
            stateEnter = false;
        }
    }


    

    #region PutState����
    private bool canCreate = false;�@//�I�u�W�F�N�g���쐬�\���ǂ����̃t���O
    private bool canObjectMove = false; //�I�u�W�F�N�g���ړ������ǂ����̃t���O
    //private Vector3 oldLayerNormal = new Vector3(0, 0, 0); //Ray�������������C���[�̖@����ۑ����A�ύX�����������ǂ������Ǘ����邽�߂̕ϐ�
    private int selectFacilityNum = 0; //�ݒu����{�݂�ID
    private GameObject createdObject; //�쐬�����{�݂�ۑ�����ϐ�
    private void UpdatePut()
    {
        //�X�e�[�g�ڍs�̍ۂɈ�񂾂����s
        if (stateEnter)
        {
            canCreate = true;
            canObjectMove = false;
        }

        //�����{�݂̕ύX����
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
            //�{�ݐ���
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                CreateFacility();
            }
        }
        else
        {
            //�{�݂̈ړ�����
            if (canObjectMove)
            {
                RaycastHit hit;

                Vector3 rayOrigin = rightController.transform.position;
                Vector3 rayDirection = rightController.transform.forward;

                if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
                {
                    //�u����ꏊ�ȊO�Ɏ{�݂����Ȃ��悤�ɂ��鏈��
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

                //�ݒu�m��or�L�����Z����UI�̕\��
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
    /// �{�݂̐���
    /// </summary>
    private void CreateFacility()
    {
        RaycastHit hit;

        Vector3 rayOrigin = rightController.transform.position;
        Vector3 rayDirection = rightController.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, ignoreLayers))
        {
            int hitLayer = hit.collider.gameObject.layer;

            //�����������C���[�̈ʒu�Ɏ{�݂��u���邩�ǂ����̏�������
            if (allowedPlacemenLayer == (allowedPlacemenLayer | (1 << hitLayer)))
            {
                GameObject createObject = allFacilityDate.facilityDates[selectFacilityNum].facilityPrefab;
                createdObject = Instantiate(createObject, hit.point, Quaternion.identity);�@//�{�݂̐ݒu
                createdObject.transform.up = hit.normal;
                canCreate = false;
                canObjectMove = true;
            }
        }
        else //�u����ꏊ�ł͂Ȃ��Ƃ��̏���
        {
            Debug.Log("�����Ȃ��ꏊ����");
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
