using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static EnemyWaveManager; //WaveState�̕ύX�ɕK�v
using System.Security;

public class UISystemManager : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager; //�Q�[���I�[�o�[����WaveState�ύX�p

    [SerializeField] private TextMeshProUGUI enemyTotalText = null;
    private int enemyTotal; //����Wave�ŏo��G�̑���

    [SerializeField] private TextMeshProUGUI defencePointText = null;
    private int defencePoint; //�f�t�F���X�\�ȓG�̐�

    public void InputEnemyTotal(int _enemyTotal)
    {
        enemyTotal += _enemyTotal;
        ChangeEnemyTotalTextt();
    }

    public void InputDefencePoint(int _defencePoint)
    {
        defencePoint = _defencePoint;
        ChangeDefencePointText();
    }

    public void UpdateEnemyTotal()
    {
        enemyTotal--;
        ChangeEnemyTotalTextt();
    }


    private void ChangeEnemyTotalTextt()
    {
        enemyTotalText.SetText(enemyTotal.ToString());
    }

    private void ChangeDefencePointText()
    {
        defencePointText.SetText(defencePoint.ToString());
    }
}
