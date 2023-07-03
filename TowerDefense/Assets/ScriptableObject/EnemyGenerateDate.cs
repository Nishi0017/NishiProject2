using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGenerateDate", menuName = "ScriptableObject/Enemy Generate Date")]
public class EnemyGenerateDate : ScriptableObject 
{
    [Tooltip("敵ID,レベル")]
    public EnemySpawnOrder[] enemySpawnOrder;

    public float spawnInterval = 1.0f;
}

[System.Serializable]
public class EnemySpawnOrder
{
    public int EnemyID;
    public int level;
}

