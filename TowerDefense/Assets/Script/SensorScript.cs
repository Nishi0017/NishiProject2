using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class SensorScript : MonoBehaviour
{
    [SerializeField] private SphereCollider searchArea = default;
    public float searchAngle = 45f;
    public float searchRadius;
    [SerializeField] private GameObject control;

    [SerializeField] private bool editor;

    private Ray ray;
    private RaycastHit hit;


    private void Start()
    {
        searchArea = GetComponent<SphereCollider>();
        searchRadius = searchArea.radius;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            var playerDirection  = other.transform.position - transform.position;

            var angle = Vector3.Angle(transform.up, playerDirection);

            if(angle <= searchAngle)
            {
                ray = new Ray(transform.position, playerDirection);
                Debug.DrawRay(ray.origin, ray.direction * searchRadius, Color.red);

                if(Physics.Raycast(ray.origin, ray.direction* searchRadius, out hit))
                {
                    if(hit.collider.CompareTag("Enemy"))
                    {
                        control.transform.position = Vector3.Lerp(control.transform.position, other.gameObject.transform.position, 0.1f);
                    }
                    else
                    {
                        control.transform.position = Vector3.Lerp(control.transform.position, transform.position, 0.1f);
                    }
                }

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
