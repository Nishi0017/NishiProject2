using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseSystemManager : MonoBehaviour
{
    public GameObject[] DefenceFacilities;

    private void Start()
    {
        InputDefenceFacilities();
    }

    public void InputDefenceFacilities()
    {
        DefenceFacilities = GameObject.FindGameObjectsWithTag("DefenceFacility");
    }
}
