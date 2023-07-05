using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    
    private bool isPlaying = true; //ゲーム中かどうかの判定
    private bool isGameCleared = false; //ゲームクリア判定
    private bool isGameOvered = false; //ゲームオーバー判定

    private int haveMoney = 0; //お金
    private int score = 0; //スコア


    //プロパティ経由で変数の値を取得する
    public static GameManager Instance { get { return instance; } }
    public bool IsPlaying { get { return isPlaying; } }
    public bool IsGameCleared { get { return isGameCleared; } }
    public bool IsGameOvered { get {  return isGameOvered; } }
    public int HaveMoney { get {  return haveMoney; } }
    public int Score { get { return score; } }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void GameCrear()
    {
        isGameCleared = true;
    }

    public void GameOver()
    {
        isGameOvered = true;
    }

    public void GetMoney(int _getMoney)
    {
        haveMoney += _getMoney;
    }

    public void UseMoney(int _useMoney)
    {
        haveMoney -= _useMoney;
    }
}
