using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyWaveManager;

public class UISystemManager : MonoBehaviour
{
    EnemyWaveManager enemyWaveManager;

    private int enemyTotal;
    private int defencePoint;

    public void InputEnemyTotal(int _enemyTotal)
    {
        enemyTotal = _enemyTotal;
    }

    public void InputDefencePoint(int _defencePoint)
    {
        defencePoint = _defencePoint;
    }

    public void UpdateEnemyTotal()
    {
        enemyTotal--;
    }

    public void UpdateEnemyPassCount()
    {
        defencePoint--;
        if(defencePoint == 0)
        {
            enemyWaveManager.ChangeState(WaveState.GameOver);
        }
    }
}
