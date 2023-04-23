using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class SensorScript : MonoBehaviour
{
    public float searchRadius = 10f;
    public float searchAngle = 70f;

    private float angle;

    [SerializeField] Transform bodyObject;
    [SerializeField] private GameObject control;

    //初期位置
    private Vector3 idlePos;

    private Collider[] hitColliders;
    [SerializeField] private List<GameObject> visibleEnemies = new List<GameObject>();

    [SerializeField] private GameObject closestEnemy;
    private GameObject target = null;
    private float closestDistance;


    private Vector3 directionToEnemy;
    private float angleToEnemy;
    private float distanceToEnemy;

    public bool canShot = false;

    private void Start()
    {
        idlePos = control.transform.position;

        searchRadius = gameObject.GetComponent<SphereCollider>().radius;

    }

    void Update()
    {
        // Find all enemies with "Enemy" tag within search radius
        hitColliders = Physics.OverlapSphere(transform.position, searchRadius, LayerMask.GetMask("Enemies"));

        visibleEnemies.Clear();
        foreach (Collider hitCollider in hitColliders)
        {
            Vector3 direction = hitCollider.transform.position - transform.position;
            direction.y = 0f;
            angle = Vector3.Angle(direction, bodyObject.forward);
            if (angle <= searchAngle * 0.5f)
            {
                // Check if there is any obstacle between the sensor and the enemy
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, searchRadius))
                {
                    if (hit.collider == hitCollider)
                    {
                        // The enemy is visible to the sensor
                        visibleEnemies.Add(hitCollider.gameObject);
                    }
                }
            }
        }

        // Find closest enemy
        if (visibleEnemies.Count > 0)
        {
            closestEnemy = null;
            closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in visibleEnemies)
            {
                directionToEnemy = (enemy.transform.position - transform.position).normalized;
                directionToEnemy.y = 0f;
                angleToEnemy = Vector3.Angle(directionToEnemy, bodyObject.forward);

                if (angleToEnemy <= searchAngle * 0.5f)
                {
                    RaycastHit hit;
                    // Cast a ray towards the enemy to check if there are any obstacles in between
                    if (Physics.Raycast(transform.position, directionToEnemy, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.gameObject == enemy)
                        {
                            distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                            if (distanceToEnemy < closestDistance)
                            {
                                closestEnemy = enemy;
                                closestDistance = distanceToEnemy;
                                if(target != closestEnemy)
                                {
                                    target = closestEnemy;
                                }
                            }
                        }
                    }
                }
            }

            // Move control object towards closest enemy
            if (closestEnemy != null)
            {
                control.transform.position = Vector3.Lerp(control.transform.position, target.transform.GetChild(0).transform.position, 0.1f);

                if (!canShot)
                {
                    canShot = true;
                }
            }
        }
        else if (control.transform.position != transform.position)
        {
            control.transform.position = Vector3.Lerp(control.transform.position, idlePos, 0.1f);

            if (canShot)
            {
                canShot = false;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        // 削除されたGameObjectにアクセスしていないことを確認する
        if (visibleEnemies.Contains(other.gameObject))
        {
            visibleEnemies.Remove(other.gameObject);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
