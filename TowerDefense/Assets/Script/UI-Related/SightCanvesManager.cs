using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SightCanvesManager : MonoBehaviour
{
    private EditFacility2 editFacility;

    [SerializeField] private TextMeshProUGUI countTimer;

    int timeLeft = 11; //�L�����Z���܂ł̎���

    void Start()
    {
        editFacility = GameObject.FindWithTag("Player").GetComponent<EditFacility2>();

        //���Ԋu�Ŋ֐����Ăяo��
        InvokeRepeating("CountTime", 0, 1);
    }

    private void CountTime()
    {
        timeLeft--;
        countTimer.SetText(timeLeft.ToString() + "...");
        if(timeLeft < 1)
        {
            PushCancelButton();
        }
        
    }

    public void PushConfirmButton()
    {
        editFacility.PutConfirm();
    }

    public void PushCancelButton()
    {
        editFacility.PutCancel();
    }
}
