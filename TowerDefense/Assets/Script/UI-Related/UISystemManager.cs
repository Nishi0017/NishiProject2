using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISystemManager : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager enemyWaveManager; //ゲームオーバー時のWaveState変更用

    [SerializeField] private TextMeshProUGUI enemyTotalText = null;
    private int enemyTotal; //あるWaveで出る敵の総数

    [SerializeField] private TextMeshProUGUI defencePointText = null;
    private int defencePoint; //デフェンス可能な敵の数

    [SerializeField] private TextMeshProUGUI haveMoneyText = null;
    private int haveMoney;

    private void Start()
    {
        haveMoney = GameManager.Instance.HaveMoney;
    }

    private void Update()
    {
        if (haveMoney != GameManager.Instance.HaveMoney)
        {
            haveMoney = GameManager.Instance.HaveMoney;
            Debug.Log("所持金:" + haveMoney);
            ChangeHaveMoneyText();
        }
    }

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
        enemyTotalText.SetText("Enemy:" + enemyTotal.ToString());
    }

    private void ChangeDefencePointText()
    {
        defencePointText.SetText("Health:" + defencePoint.ToString());
    }

    private void ChangeHaveMoneyText()
    {
        haveMoneyText.SetText("Money" + haveMoney.ToString());
    }
}
