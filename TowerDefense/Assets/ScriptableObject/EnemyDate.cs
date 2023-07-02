using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDate", menuName = "ScriptableObject/Enemy Date")]
public class EnemyDate : ScriptableObject
{
    public GameObject prefab; //敵のプレハブ
    public string enemyName; //名前
    public int health; //体力
    public int attackPower; //攻撃力
    public int defensePower; //防御力
    public float speed; //スピード
    public float reward; //報酬(お金、素材？)
    public float experience; //経験値
}
