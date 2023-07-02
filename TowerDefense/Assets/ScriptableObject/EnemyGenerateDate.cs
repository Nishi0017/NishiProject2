using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGenerateDate", menuName = "ScriptableObject/Enemy Generate Date")]
public class EnemyGenerateDate : ScriptableObject 
{
    [Tooltip("数字のみ入力してください")]
    public char[] enemySpawnOrder;

    public float spawnInterval = 1.0f;
}

