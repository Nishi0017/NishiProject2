using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDate", menuName = "ScriptableObject/Enemy Date")]
public class EnemyDate : ScriptableObject
{
    public GameObject prefab; //�G�̃v���n�u
    public string enemyName; //���O
    public int health; //�̗�
    public int attackPower; //�U����
    public int defensePower; //�h���
    public float speed; //�X�s�[�h
    public float reward; //��V(�����A�f�ށH)
    public float experience; //�o���l
}
