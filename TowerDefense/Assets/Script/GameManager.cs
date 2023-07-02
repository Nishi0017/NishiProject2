using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //ゲーム中かどうかの判定
    private bool isPlaying = true;
    //ゲームクリア判定
    private bool isGameCleared = false;
    //ゲームオーバー判定
    private bool isGameOvered = false;

    //プロパティ経由で変数の値を取得する
    public static GameManager Instance { get { return instance; } }
    public bool IsPlaying { get { return isPlaying; } }
    public bool IsGameCleared { get { return isGameCleared; } }
    public bool IsGameOvered { get {  return isGameOvered; } }


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
}
