using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCanvesManager : MonoBehaviour
{
    EditFacility2 editFacility;
    void Start()
    {
        editFacility = GameObject.FindWithTag("Player").GetComponent<EditFacility2>();
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
