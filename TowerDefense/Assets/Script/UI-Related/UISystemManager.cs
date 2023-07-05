using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static EnemyWaveManager; //WaveStateの変更に必要
using System.Security;

public class UISystemManager : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager; //ゲームオーバー時のWaveState変更用

    [SerializeField] private TextMeshProUGUI enemyTotalText = null;
    private int enemyTotal; //あるWaveで出る敵の総数

    [SerializeField] private TextMeshProUGUI defencePointText = null;
    private int defencePoint; //デフェンス可能な敵の数

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
