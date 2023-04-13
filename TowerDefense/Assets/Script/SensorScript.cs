using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SensorScript : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea = default;
    [SerializeField] private float searchAngle = 45f;
    [SerializeField] private GameObject control;

    [SerializeField] private bool editor;

    // private void OnTriggerStay(Collider other)
    // {
    //     if(other.gameObject.tag == "Enemy")
    //     {
    //         control.transform.position = Vector3.Lerp(control.transform.position, other.gameObject.transform.position, 0.1f);
    //     }
    //     else
    //     {
    //         control.transform.position = Vector3.Lerp(control.transform.position, transform.position, 0.1f);
    //     }
    // }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            var playerDirection  = other.transform.position - transform.position;

            var angle = Vector3.Angle(transform.up, playerDirection);

            if(angle <= searchAngle)
            {
                control.transform.position = Vector3.Lerp(control.transform.position, other.gameObject.transform.position, 0.1f);
            }
            else if(angle > searchAngle)
            {
                control.transform.position = Vector3.Lerp(control.transform.position, transform.position, 0.1f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        control.transform.position = transform.position;
    }
    
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.right, Quaternion.Euler(-searchAngle, 0f, 0f) * transform.up, searchAngle * 2f, searchArea.radius * 0.1f);
    }
}
