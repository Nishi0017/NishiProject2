using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemyScript : MonoBehaviour
{
    private bool canGenerate = false;

    [SerializeField] private Transform generatePos;
    [SerializeField] private Transform goalPos;
    [SerializeField] private float generateTime = 1.0f; 

    [SerializeField] private GameObject[] generateEnemy;
    private int generateEnemyLength;
    [SerializeField] private int[] generateNumber;

    private float time = 0;
    private GameObject enemy;
    private int count = 0;
    private int enemyNumber = 0;

    private void Start()
    {
        generateEnemyLength = generateEnemy.Length;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            canGenerate = true;
        }

        if(canGenerate )
        {
            time += Time.deltaTime;
            if (time > generateTime)
            {
                enemy = Instantiate(generateEnemy[enemyNumber], generatePos.position, Quaternion.identity);
                enemy.GetComponent<Test_HumanScript>().goalPosition = goalPos;
                count++;
                enemy.name = "Enemy" + count;
                
                if (count >= generateNumber[enemyNumber])
                {
                    enemyNumber++;
                    count = 0;
                    if (enemyNumber >= generateEnemyLength)
                    {
                        canGenerate = false;
                    }
                }
                time = 0;
            }
        }

    }

    public void Generate()
    {
        while (true)
        {
            time += Time.deltaTime;
            if(time > generateTime)
            {
                enemy = Instantiate(generateEnemy[enemyNumber], generatePos.position, Quaternion.identity);
                enemy.GetComponent<Test_HumanScript>().goalPosition = goalPos;
                count++;
                time = 0;
                if(count == generateNumber[enemyNumber])
                {
                    enemyNumber++;
                    count = 0;
                    if(enemyNumber > generateEnemyLength)
                    {
                        break;
                    }
                }
            }
        }
    }
}
